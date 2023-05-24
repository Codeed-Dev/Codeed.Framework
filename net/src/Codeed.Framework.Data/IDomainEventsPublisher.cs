using Codeed.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeed.Framework.Data
{
    public interface IDomainEventsPublisher
    {
        Task Publish(IEnumerable<Event> events);
    }
}
