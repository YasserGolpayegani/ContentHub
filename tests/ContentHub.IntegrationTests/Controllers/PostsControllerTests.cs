using System.Net;
using System.Net.Http.Json;
using ContentHub.Application.Features.Posts.Commands.CreatePost;
using ContentHub.Domain.Entities;
using ContentHub.IntegrationTests.Infrastructure;
using ContentHub.Persistence.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContentHub.IntegrationTests.Controllers;

public sealed class PostsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PostsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task Create_ShouldReturnCreated_WhenUserHasPermission()
    {
        // Arrange
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "create-permission");

        var categoryId = await CreateCategoryAsync();

        var command = new CreatePostCommand(
            "Integration Test Post",
            "Integration test post content",
            "integration-test-post",
            categoryId);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var postId = await response.Content.ReadFromJsonAsync<Guid>();

        postId.Should().NotBeEmpty();

        response.Headers.Location.Should().NotBeNull();
    }


    [Fact]
    public async Task Create_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "none");

        var categoryId = await CreateCategoryAsync();

        var command = new CreatePostCommand(
            "Unauthorized Post",
            "Unauthorized post content",
            "unauthorized-post",
            categoryId);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task Create_ShouldReturnForbidden_WhenUserDoesNotHavePermission()
    {
        // Arrange
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "authenticated");

        var categoryId = await CreateCategoryAsync();

        var command = new CreatePostCommand(
            "Forbidden Post",
            "Forbidden post content",
            "forbidden-post",
            categoryId);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }


    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenTitleIsEmpty()
    {
        // Arrange
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "create-permission");

        var categoryId = await CreateCategoryAsync();

        var command = new CreatePostCommand(
            string.Empty,
            "Integration test content",
            "invalid-post",
            categoryId);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task Create_ShouldPersistPost_WhenRequestIsValid()
    {
        // Arrange
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "create-permission");

        var categoryId = await CreateCategoryAsync();

        var command = new CreatePostCommand(
            "Persisted Integration Post",
            "Persisted integration test content",
            "persisted-integration-post",
            categoryId);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var postId = await response.Content.ReadFromJsonAsync<Guid>();

        postId.Should().NotBeEmpty();

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var post = await dbContext.Posts.FindAsync(postId);

        post.Should().NotBeNull();
        post!.Title.Should().Be("Persisted Integration Post");
        post.Content.Should().Be("Persisted integration test content");
        post.Slug.Should().Be("persisted-integration-post");
        post.CategoryId.Should().Be(categoryId);
        post.IsPublished.Should().BeFalse();
    }


    [Fact]
    public async Task Create_ShouldNotPersistPost_WhenValidationFails()
    {
        // Arrange
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "create-permission");

        var categoryId = await CreateCategoryAsync();

        var command = new CreatePostCommand(
            string.Empty,
            "Integration test content",
            $"invalid-{Guid.NewGuid():N}",
            categoryId);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var posts = await dbContext.Posts.Where(x => x.Slug == command.Slug).ToListAsync();

        posts.Should().BeEmpty();
    }


    private async Task<Guid> CreateCategoryAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var category = new Category(
            "Integration Test Category",
            $"integration-test-category-{Guid.NewGuid():N}");

        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();

        return category.Id;
    }


    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenUserIsAnonymous()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/Posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}