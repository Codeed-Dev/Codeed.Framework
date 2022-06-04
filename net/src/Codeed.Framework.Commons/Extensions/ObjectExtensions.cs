using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Codeed.Framework.Commons.Extensions
{
    public static class ObjectExtensions
    {
        public static void ChangePropertyValue<T>(this T entidade, Expression<Func<T, object>> expression, object value) where T : class
        {
            if (expression == null)
                return;

            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(entidade, value);
                }
            }
        }

        public static IList<T> ObjectToList<T>(this T obj)
        {
            return new List<T> { obj };
        }
    }
}
