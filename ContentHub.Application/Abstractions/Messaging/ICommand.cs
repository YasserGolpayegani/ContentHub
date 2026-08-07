using MediatR;

namespace ContentHub.Application.Abstractions.Messaging;

/// <summary>
/// Represents a command without a response.
/// </summary>
public interface ICommand : IRequest, ICommandBase
{

}

/// <summary>
/// Represents a command with a response.
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase
{

}
