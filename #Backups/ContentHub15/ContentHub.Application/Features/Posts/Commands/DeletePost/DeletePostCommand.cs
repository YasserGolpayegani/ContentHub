using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands.DeletePost;

public sealed record DeletePostCommand(Guid id) : IRequest;
