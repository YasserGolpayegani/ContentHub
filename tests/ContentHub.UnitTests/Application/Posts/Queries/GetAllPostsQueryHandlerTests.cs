using ContentHub.Application.Abstractions.Caching;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Constants;
using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Features.Posts.Queries.GetAllPostsQuery;
using ContentHub.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ContentHub.UnitTests.Application.Posts.Queries;

public class GetAllPostsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCachedPosts_WhenPostsAreCached()
    {
        // Arrange
        var cachedPosts = new List<PostDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Cached Post",
                Content = "Cached post content",
                Slug = "cached-post",
                IsPublished = true,
                CreatedOnUtc = DateTime.UtcNow
            }
        };

        var postRepositoryMock = new Mock<IPostRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        cacheServiceMock
            .Setup(cacheService => cacheService.GetAsync<List<PostDto>>(CacheKeys.Posts))
            .ReturnsAsync(cachedPosts);

        var handler = new GetAllPostsQueryHandler(
            postRepositoryMock.Object,
            cacheServiceMock.Object);

        var query = new GetAllPostsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        result[0].Title.Should().Be("Cached Post");
        result[0].Slug.Should().Be("cached-post");

        postRepositoryMock.Verify(repository =>
            repository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);

        cacheServiceMock.Verify(cacheService =>
            cacheService.GetAsync<List<PostDto>>(CacheKeys.Posts), Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldReturnPostsFromRepository_WhenPostsAreNotCached()
    {
        // Arrange
        var posts = new List<Post>
        {
            new("First Post","First post content","first-post",Guid.NewGuid()),
            new("Second Post","Second post content","second-post",Guid.NewGuid()),
         };

        var postRepositoryMock = new Mock<IPostRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        cacheServiceMock.Setup(cacheService => cacheService
        .GetAsync<List<PostDto>>(CacheKeys.Posts))
            .ReturnsAsync((List<PostDto>?)null);

        postRepositoryMock.Setup(repository => repository
            .GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(posts);

        var handler = new GetAllPostsQueryHandler(postRepositoryMock.Object, cacheServiceMock.Object);

        var query = new GetAllPostsQuery();

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Title.Should().Be("First Post");
        result[0].Slug.Should().Be("first-post");

        result[1].Title.Should().Be("Second Post");
        result[1].Slug.Should().Be("second-post");

        postRepositoryMock.Verify(repository => repository
            .GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);

        cacheServiceMock.Verify(cacheService => cacheService
            .GetAsync<List<PostDto>>(CacheKeys.Posts), Times.Once);

        cacheServiceMock.Verify(cacheService => cacheService
            .SetAsync(CacheKeys.Posts, It.IsAny<List<PostDto>>(), TimeSpan.FromMinutes(5)),Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenPostsAreNotCachedAndRepositoryIsEmpty()
    {
        // Arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        cacheServiceMock
            .Setup(cacheService => cacheService.GetAsync<List<PostDto>>(CacheKeys.Posts))
            .ReturnsAsync((List<PostDto>?)null);

        postRepositoryMock
            .Setup(repository => repository.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Post>());

        var handler = new GetAllPostsQueryHandler(
            postRepositoryMock.Object,
            cacheServiceMock.Object);

        var query = new GetAllPostsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        postRepositoryMock.Verify(repository => repository
        .GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        cacheServiceMock.Verify(cacheService => cacheService
        .SetAsync(CacheKeys.Posts, It.IsAny<List<PostDto>>(), TimeSpan.FromMinutes(5)),
            Times.Once);
    }
}