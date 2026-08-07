using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Common;
using ContentHub.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands.DeletePost;

public sealed record DeletePostCommand(Guid id)
    : ICommand, IAuditableCommand
{
    public string Action => AuditActions.Delete;
    public string EntityName => nameof(Post);
    public string? EntityId => id.ToString();
}