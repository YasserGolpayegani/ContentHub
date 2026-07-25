using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands.UpdatePost;

public sealed record UpdatePostCommand(
    Guid Id,
    string Title,
    string Content,
    string Slug,
    Guid CategoryId) : IRequest;

