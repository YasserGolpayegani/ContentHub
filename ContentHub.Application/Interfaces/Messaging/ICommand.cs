using MediatR;

namespace ContentHub.Application.Interfaces.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
