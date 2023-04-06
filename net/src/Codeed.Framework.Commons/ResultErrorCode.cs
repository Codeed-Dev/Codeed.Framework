using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public class ResultErrorCode
    {
        public ResultErrorCode(string errorCode, Dictionary<string, object>? parameters)
        {
            ErrorCode = errorCode;
            Parameters = parameters ?? new Dictionary<string, object>();
        }
        public string ErrorCode { get;  }

        public Dictionary<string, object> Parameters { get;  }
    }
}