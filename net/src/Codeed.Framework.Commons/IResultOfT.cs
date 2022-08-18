using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public interface IResultOfT<out T> : IResult
    {
        T Value { get; }
    }
}
