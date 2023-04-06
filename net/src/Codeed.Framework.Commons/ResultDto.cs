using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public class ResultDto<T> : IResultOfT<T>
    {
        public T? Value { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public IEnumerable<ResultErrorCode> ErrorCodes { get; set; } = new List<ResultErrorCode>();

        public bool Succeeded { get; set; }

        public bool Failed { get; set; }
    }
}
