using Codeed.Framework.Domain;
using Codeed.Framework.EventBus;
using Codeed.Framework.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeed.Framework.Data
{
    public class EventBusDomainEventsPublisher : IDomainEventsPublisher
    {
        private IEventBus _eventBus;
        private ITenantService _tenantService;

        public EventBusDomainEventsPublisher(IEventBus eventBus, ITenantService tenantService)
        {
            _eventBus = eventBus;
            _tenantService = tenantService;
        }

        public async Task Publish(IEnumerable<Event> events)
        {
            if (_eventBus is null || events is null || !events.Any())
                return;

            var publishEventErrors = new List<Exception>();
            foreach (var domainEvent in events)
            {
                try
                {
                    if (domainEvent is ITenantEvent tenantEvent)
                    {
                        await _eventBus.Publish(tenantEvent, _tenantService).ConfigureAwait(false);
                    }
                    else
                    {
                        await _eventBus.Publish(domainEvent).ConfigureAwait(false);
                    }
                }
                catch (Exception e)
                {
                    publishEventErrors.Add(e);
                }
            }

            var firstError = publishEventErrors.FirstOrDefault();
            if (firstError != null)
            {
                throw firstError;
            }
        }
    }
}
