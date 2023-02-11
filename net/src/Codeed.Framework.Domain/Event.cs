using MediatR;
using System;
using System.Text.Json.Serialization;

namespace Codeed.Framework.Domain
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        public Guid Id { get; private set; }

        protected Event()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        [JsonConstructor]
        public Event(Guid id, DateTime timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}