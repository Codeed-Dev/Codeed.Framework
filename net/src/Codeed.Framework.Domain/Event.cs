using System;
using System.Text.Json.Serialization;

namespace Codeed.Framework.Domain
{
    public abstract class Event : Message, IEvent
    {
        public DateTimeOffset Timestamp { get; private set; }

        public Guid Id { get; private set; }

        protected Event()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }

        [JsonConstructor]
        protected Event(Guid id, DateTimeOffset timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}