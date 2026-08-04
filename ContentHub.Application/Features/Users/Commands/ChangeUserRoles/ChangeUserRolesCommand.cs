using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Users.Commands.ChangeUserRoles;

public record class ChangeUserRolesCommand(Guid UserId, IReadOnlyCollection<string> Roles) : IRequest;
