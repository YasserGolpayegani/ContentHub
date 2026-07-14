using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
