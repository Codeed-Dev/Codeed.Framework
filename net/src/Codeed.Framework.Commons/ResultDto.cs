using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public class ResultDto<T> : IResult<T>
    {
        public T Value { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public IEnumerable<ResultErrorCode> ErrorCodes { get; set; }

        public bool Succeeded { get; set; }

        public bool Failed { get; set; }
    }
}
