using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands;

public sealed record CreatePostCommand(
    string Title,
    string Content,
    string Slug,
    Guid CategoryId) : IRequest<Guid>;

