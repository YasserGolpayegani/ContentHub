using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Common;
using ContentHub.Domain.Entities;
using ContentHub.Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands.CreatePost;

public sealed record CreatePostCommand(
    string Title,
    string Content,
    string Slug,
    Guid CategoryId) : ICommand<Guid>;