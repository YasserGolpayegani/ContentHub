using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Authentication.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest;
