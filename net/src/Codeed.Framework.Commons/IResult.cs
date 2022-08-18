using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public interface IResult
    {
        IEnumerable<string> Errors { get; }

        IEnumerable<ResultErrorCode> ErrorCodes { get; }

        bool Succeeded { get; }

        bool Failed { get; }
    }
}