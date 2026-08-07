using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.DTOs.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;
