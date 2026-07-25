using ContentHub.Application.Features.Authentication.Commands.Login;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ContentHub.Application.Features.Authentication.Commands.Refresh;

public sealed record RefreshCommand(string RefreshToken) : IRequest<LoginResponse>;
