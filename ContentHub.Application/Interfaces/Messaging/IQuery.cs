using MediatR;

namespace ContentHub.Application.Interfaces.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{

}
