using MediatR;

namespace Codeed.Framework.EventBus
{
    public interface IRequestReplyBroker
    {
        Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
    }
}
