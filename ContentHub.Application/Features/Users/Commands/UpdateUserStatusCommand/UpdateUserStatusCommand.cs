using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Common;
using ContentHub.Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace ContentHub.Application.Features.Users.Commands.UpdateUserStatusCommand;

public sealed record UpdateUserStatusCommand(Guid UserId, bool IsActive)
    : ICommand, IAuditableCommand
{
    public string Action => AuditActions.Update;
    public string EntityName => nameof(AppUser);
    public string? EntityId => UserId.ToString();
}