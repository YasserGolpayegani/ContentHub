using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Guid>
;
