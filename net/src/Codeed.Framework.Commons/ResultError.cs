using System;

namespace Codeed.Framework.Commons
{
    public class ResultError
    {
        public ResultError(string errorMessage)
        {
            Message = errorMessage;
        }

        public ResultError(Exception exception) : this(exception.Message)
        {
            Exception = exception;
        }

        public ResultError(string errorMessage, ResultErrorCode? errorCode)
        {
            Message = errorMessage;
            ErrorCode = errorCode;
        }

        public ResultError(Exception exception, ResultErrorCode? errorCode) : this(exception.Message, errorCode)
        {
            Exception = exception;
        }

        public string Message { get; private set; } = "";

        public Exception? Exception { get; private set; }

        public ResultErrorCode? ErrorCode { get; private set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
