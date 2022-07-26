using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public class ResultErrorCode
    {
        public ResultErrorCode(string errorCode, Dictionary<string, object> parameters)
        {
            ErrorCode = errorCode;
            Parameters = parameters;
        }
        public string ErrorCode { get;  }

        public Dictionary<string, object> Parameters { get;  }
    }
}