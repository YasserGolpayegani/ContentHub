using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.DTOs.Posts;

namespace ContentHub.Application.Features.Posts.Queries.GetAllPostsQuery;

public sealed record GetAllPostsV2Query() : IQuery<IReadOnlyList<PostV2Dto>>;
