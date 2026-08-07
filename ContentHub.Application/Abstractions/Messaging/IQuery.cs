using MediatR;

namespace ContentHub.Application.Abstractions.Messaging;

/// <summary>
/// Represents a query.
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse>
{

}
