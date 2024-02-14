using MediatR;

namespace Codeed.Framework.EventBus
{
    public class MediatRRequestReplyBroker : IRequestReplyBroker
    {
        private readonly IMediator _mediator;

        public MediatRRequestReplyBroker(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }
    }
}
