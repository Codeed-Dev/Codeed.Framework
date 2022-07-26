using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        public static void ChangePropertyValue<T>(this T entidade, Expression<Func<T, object>> expression, object value) where T : class
        {
            if (expression == null)
            {
                return;
            }

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

        public static Dictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {
            if (obj == null)
            {
                return new Dictionary<string, TValue>();
            }

            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
            return dictionary;
        }

        public static IList<T> ObjectToList<T>(this T obj)
        {
            return new List<T> { obj };
        }
    }
}
