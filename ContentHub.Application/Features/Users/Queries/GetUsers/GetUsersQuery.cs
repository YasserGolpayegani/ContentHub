using ContentHub.Application.DTOs.Users;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery() : IRequest<IReadOnlyCollection<UserDto>>;

