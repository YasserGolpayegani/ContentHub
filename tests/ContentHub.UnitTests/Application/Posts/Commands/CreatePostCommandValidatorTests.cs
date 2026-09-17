using ContentHub.Application.Features.Posts.Commands.CreatePost;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.UnitTests.Application.Posts.Commands;

public class CreatePostCommandValidatorTests
{
    private readonly CreatePostCommandValidator _validator = new();

    [Fact]
    public async Task Validate_ShouldPass_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreatePostCommand(
            "Test Post", 
            "Test post content", 
            "test-post", 
            Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }


    [Fact]
    public async Task Validate_ShouldFail_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreatePostCommand(
            string.Empty, 
            "Test post content", 
            "test-post", 
            Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle(error =>
            error.PropertyName == nameof(CreatePostCommand.Title) &&
            error.ErrorCode == "NotEmptyValidator");
    }


    [Fact]
    public async Task Validate_ShouldFail_WhenTitleExceedsMaximumLength()
    {
        // Arrange
        var command = new CreatePostCommand(
            new string('A', 201),
            "Test post content", 
            "test-post", 
            Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle(error =>
            error.PropertyName == nameof(CreatePostCommand.Title) &&
            error.ErrorCode == "MaximumLengthValidator");
    }


    [Fact]
    public async Task Validate_ShouldPass_WhenTitleHasMaximumAllowedLength()
    {
        // Arrange
        var command = new CreatePostCommand(
            new string('A', 200),
            "Test post content",
            "test-post",
            Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }


    [Fact]
    public async Task Validate_ShouldFail_WhenContentIsEmpty()
    {
        // Arrange
        var command = new CreatePostCommand(
            "Test Post",
            string.Empty,
            "test-post",
            Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle(error =>
            error.PropertyName == nameof(CreatePostCommand.Content) &&
            error.ErrorCode == "NotEmptyValidator");
    }


    [Fact]
    public async Task Validate_ShouldFail_WhenSlugIsEmpty()
    {
        // Arrange
        var command = new CreatePostCommand(
            "Test Post",
            "Test post content",
            string.Empty,
            Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle(error =>
            error.PropertyName == nameof(CreatePostCommand.Slug) &&
            error.ErrorCode == "NotEmptyValidator");
    }


    [Fact]
    public async Task Validate_ShouldFail_WhenCategoryIdIsEmpty()
    {
        // Arrange
        var command = new CreatePostCommand(
            "Test Post",
            "Test post content",
            "test-post",
            Guid.Empty);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle(error =>
            error.PropertyName == nameof(CreatePostCommand.CategoryId) &&
            error.ErrorCode == "NotEmptyValidator");
    }
}
