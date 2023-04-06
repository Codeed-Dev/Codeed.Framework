using System;

namespace Codeed.Framework.Commons.Exceptions
{
    public abstract class BaseServiceException : Exception, IHttpException
    {
        protected BaseServiceException()
        {

        }

        protected BaseServiceException(string errorMessage) : base(errorMessage)
        {

        }

        protected BaseServiceException(string errorMessage, string code) : base(errorMessage)
        {
            Code= code;
        }

        protected BaseServiceException(string errorMessage, string code, object parameters) : base(errorMessage)
        {
            Code = code;
            Parameters = parameters;
        }

        public string Code { get; } = "";

        public object? Parameters { get; }

        public abstract int HttpCode { get; }
    }
}
