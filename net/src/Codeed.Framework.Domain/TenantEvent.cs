using System;
using System.Text.Json.Serialization;

namespace Codeed.Framework.Domain
{
    public abstract class TenantEvent : Event, ITenantEvent
    {
        public string Tenant { get; set; }

        public TenantEvent() : base()
        {
        }

        [JsonConstructor]
        protected TenantEvent(Guid id, DateTime timestamp, string tenant) : base(id, timestamp)
        {
            Tenant = tenant;
        }
    }
}