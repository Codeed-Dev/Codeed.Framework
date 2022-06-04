using System;
using System.Collections.Generic;

namespace Codeed.Framework.Domain
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; protected set; }

        public DateTimeOffset CreatedDate { get; protected set; }


        private List<Event> _domainEvents = new List<Event>();

        public IReadOnlyCollection<Event> Events => _domainEvents.AsReadOnly();


        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTimeOffset.UtcNow;
        }

        public void AddDomainEvent(Event @event)
        {
            _domainEvents = _domainEvents ?? new List<Event>();
            _domainEvents.Add(@event);
        }

        public void RemoveDomainEvent(Event domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }


        #region BaseBehaviours

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() ^ 93) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        #endregion
    }
}
