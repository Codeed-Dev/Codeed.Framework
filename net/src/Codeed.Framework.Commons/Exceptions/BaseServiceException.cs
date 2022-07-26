using System;

namespace Codeed.Framework.Commons.Exceptions
{
    public abstract class BaseServiceException : Exception, IHttpException
    {
        public BaseServiceException() : base()
        {

        }

        public BaseServiceException(string errorMessage) : base(errorMessage)
        {

        }

        public BaseServiceException(string errorMessage, string code) : base(errorMessage)
        {
            Code= code;
        }

        public BaseServiceException(string errorMessage, string code, object parameters) : base(errorMessage)
        {
            Code = code;
            Parameters = parameters;
        }

        public string Code { get; }

        public object Parameters { get; }

        public abstract int HttpCode { get; }
    }
}
