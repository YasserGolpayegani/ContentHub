using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Common;
using ContentHub.Domain.Entities;
using ContentHub.Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands.UpdatePost;

public sealed record UpdatePostCommand(
    Guid Id,
    string Title,
    string Content,
    string Slug,
    Guid CategoryId) : ICommand,IAuditableCommand
{
    public string Action => AuditActions.Update;

    public string EntityName => nameof(Post);

    public string? EntityId => Id.ToString();
}