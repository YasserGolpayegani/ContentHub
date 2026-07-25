using ContentHub.Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Queries.GetAllPostsQuery;

public sealed record GetAllPostsQuery() : IRequest<IReadOnlyList<PostDto>>;


