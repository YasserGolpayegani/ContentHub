using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Common;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContentHub.Application.Features.Authentication.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>,IAuditableCommand
{
    public string Action => AuditActions.Login;

    public string EntityName => nameof(AppUser);

    public string? EntityId => null;
}