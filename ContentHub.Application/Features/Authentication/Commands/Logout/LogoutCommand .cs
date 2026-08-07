using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Common;
using ContentHub.Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Authentication.Commands.Logout;

public record LogoutCommand(string RefreshToken) : ICommand,IAuditableCommand
{
    public string Action => AuditActions.Logout;

    public string EntityName => nameof(AppUser);

    public string? EntityId => null;
}