using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeed.Framework.Commons
{
    public abstract class EquatableObject<T> : IEquatable<T>
        where T : EquatableObject<T>
    {
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var other = (T)obj;
            return Equals(other);
        }


        public virtual bool Equals(T? other)
        {
            if (other is null)
                return false;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }


        protected static bool Compare(T left, T right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return left is null || left.Equals(right);
        }
    }
}
