﻿using System.Linq.Expressions;

namespace Codeed.Framework.Specification
{
    internal sealed class IdentitySpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
        }
    }
}