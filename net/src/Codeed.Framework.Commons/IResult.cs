using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public interface IResult<T> : IResult
    {
        T Value { get; }
    }
}
