using ContentHub.Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ContentHub.Application.Features.Posts.Queries.GetPostById;

public sealed record GetPostByIdQuery(Guid Id) : IRequest<PostDto>;

