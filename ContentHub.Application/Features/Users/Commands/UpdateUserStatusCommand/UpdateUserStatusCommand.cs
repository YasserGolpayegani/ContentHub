using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Users.Commands.UpdateUserStatusCommand;

public sealed record UpdateUserStatusCommand(Guid UserId, bool IsActive) : IRequest;
