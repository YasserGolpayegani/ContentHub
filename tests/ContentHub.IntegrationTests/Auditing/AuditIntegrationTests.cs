using System.Net;
using System.Net.Http.Json;
using ContentHub.Domain.Entities;
using ContentHub.IntegrationTests.Infrastructure;
using ContentHub.Persistence.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContentHub.IntegrationTests.Auditing;

public sealed class AuditIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuditIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreatePost_ShouldCreateSuccessfulAuditLog_WhenPostIsCreated()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "create-permission");

        var categoryId = await CreateCategoryAsync();

        var command = new
        {
            Title = "Audited Integration Post",
            Content = "Content for audit integration test.",
            Slug = $"audited-post-{Guid.NewGuid():N}",
            CategoryId = categoryId
        };

        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var postId = await response.Content.ReadFromJsonAsync<Guid>();

        postId.Should().NotBeEmpty();

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var auditLogs = await dbContext.AuditLogs
            .AsNoTracking()
            .Where(x => x.EntityName == "Post" && x.EntityId == postId.ToString())
            .OrderByDescending(x => x.TimestampUtc)
            .ToListAsync();

        auditLogs.Should().NotBeEmpty();

        var auditLog = auditLogs.First();

        auditLog.EntityName.Should().Be("Post");
        auditLog.EntityId.Should().Be(postId.ToString());
        auditLog.IsSuccessful.Should().BeTrue();
        auditLog.TimestampUtc.Should().NotBe(default);
    }


    [Fact]
    public async Task CreatePost_ShouldCreateEntityAuditLog_WhenPostIsPersisted()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Test-Authentication", "create-permission");

        var categoryId = await CreateCategoryAsync();

        var command = new
        {
            Title = "Entity Audit Test",
            Content = "Testing entity level auditing.",
            Slug = $"entity-audit-{Guid.NewGuid():N}",
            CategoryId = categoryId
        };

        var response = await client.PostAsJsonAsync("/api/v1/Posts", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var postId = await response.Content.ReadFromJsonAsync<Guid>();

        postId.Should().NotBeEmpty();

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var auditLogs = await dbContext.AuditLogs
            .AsNoTracking()
            .Where(x => x.EntityName == "Post" && x.EntityId == postId.ToString())
            .OrderByDescending(x => x.TimestampUtc)
            .ToListAsync();

        auditLogs.Should().NotBeEmpty();

        var entityAuditLog = auditLogs.FirstOrDefault(x => x.Action == "Added");

        entityAuditLog.Should().NotBeNull();

        entityAuditLog!.EntityName.Should().Be("Post");
        entityAuditLog.EntityId.Should().Be(postId.ToString());
        entityAuditLog.IsSuccessful.Should().BeTrue();
        entityAuditLog.FailureReason.Should().BeNull();
    }


    private async Task<Guid> CreateCategoryAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var category = new Category(
            "Audit Integration Category",
            $"audit-category-{Guid.NewGuid():N}");

        await dbContext.Categories.AddAsync(category);

        await dbContext.SaveChangesAsync();

        return category.Id;
    }
}