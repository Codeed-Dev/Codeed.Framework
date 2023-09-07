using Codeed.Framework.Domain;
using Codeed.Framework.Tenant;
using MediatR;

namespace Codeed.Framework.Data
{
    public class MediatorDomainEventsPublisher : IDomainEventsPublisher
    {
        private readonly IMediator _mediator;
        private readonly ITenantService _tenantService;

        public MediatorDomainEventsPublisher(IMediator mediator, ITenantService tenantService)
        {
            _mediator = mediator;
            _tenantService = tenantService;
        }

        public async Task Publish(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                if (@event is ITenantEvent tenantEvent)
                {
                    tenantEvent.Tenant = _tenantService.Tenant;
                }

                await _mediator.Publish(@event);
            }
        }
    }
}
