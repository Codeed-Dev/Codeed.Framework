using Codeed.Framework.Commons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codeed.Framework.Domain
{
    public abstract class Message : EquatableObject<Message>
    {
        public string MessageType { get; protected set; }

        protected virtual bool Equatable { get; }

        protected Message()
        {
            MessageType = GetType().Name;
        }

        public override bool Equals(Message? other)
        {
            if (other is null)
                return false;

            return Equatable ? base.Equals(other) : other == this;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return MessageType;
        }
    }
}
