using ContentHub.Application.Abstractions.Caching;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Constants;
using ContentHub.Application.Features.Posts.Commands.CreatePost;
using ContentHub.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.UnitTests.Application.Posts.Commands;

public class CreatePostCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddPostAndRemoveCache_WhenCommandIsValid()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var command = new CreatePostCommand("Test Post", "Test post content", "test-post", categoryId);

        var postRepositoryMock = new Mock<IPostRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        Post? addedPost = null;

        postRepositoryMock.Setup(repository => repository.AddAsync(
            It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Callback<Post, CancellationToken>((post, _) =>
            {
                addedPost = post;
            })
            .Returns(Task.CompletedTask);

        var handler = new CreatePostCommandHandler(postRepositoryMock.Object, cacheServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert   
        addedPost.Should().NotBeNull();
        addedPost!.Id.Should().Be(result);
        addedPost.Title.Should().Be("Test Post");
        addedPost.Content.Should().Be("Test post content");
        addedPost.Slug.Should().Be("test-post");
        addedPost.CategoryId.Should().Be(categoryId);
        addedPost.IsPublished.Should().BeFalse();

        postRepositoryMock.Verify(repository => repository
        .AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);

        cacheServiceMock.Verify(cacheService => cacheService
        .RemoveAsync(CacheKeys.Posts), Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var command = new CreatePostCommand("Test Post", "Test post content", "test-post", categoryId);

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var postRepositoryMock = new Mock<IPostRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        postRepositoryMock.Setup(repository => repository
            .AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreatePostCommandHandler(postRepositoryMock.Object, cacheServiceMock.Object);

        // Act
        await handler.Handle(command, cancellationToken);

        // Assert
        postRepositoryMock.Verify(repository => repository
            .AddAsync(It.IsAny<Post>(), cancellationToken), Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldRemovePostsCacheAfterAddingPost()
    {
        // Arrange
        var command = new CreatePostCommand("Test Post", "Test post content", "test-post", Guid.NewGuid());

        var postRepositoryMock = new Mock<IPostRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        postRepositoryMock.Setup(repository => repository
            .AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreatePostCommandHandler(postRepositoryMock.Object, cacheServiceMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        cacheServiceMock.Verify(cacheService => cacheService
            .RemoveAsync(CacheKeys.Posts), Times.Once);
    }
}
