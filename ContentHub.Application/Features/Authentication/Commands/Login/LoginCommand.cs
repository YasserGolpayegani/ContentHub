using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Authentication.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
