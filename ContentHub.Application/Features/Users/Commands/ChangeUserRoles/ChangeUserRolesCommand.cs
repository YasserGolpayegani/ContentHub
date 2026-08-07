using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Common;
using ContentHub.Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Users.Commands.ChangeUserRoles;

public record class ChangeUserRolesCommand(Guid UserId, IReadOnlyCollection<string> Roles) : ICommand,IAuditableCommand
{
    public string Action => AuditActions.ChangeRole;
    public string EntityName => nameof(AppUser);
    public string? EntityId => UserId.ToString();
}