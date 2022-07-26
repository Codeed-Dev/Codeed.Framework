using System;

namespace Codeed.Framework.Commons.Exceptions
{
    public class ServiceForbiddenException : BaseServiceException
    {
        public ServiceForbiddenException() : base()
        {

        }

        public ServiceForbiddenException(string errorMessage) : base(errorMessage)
        {

        }

        public ServiceForbiddenException(string errorMessage, string code) : base(errorMessage, code)
        {
        }

        public ServiceForbiddenException(string errorMessage, string code, object parameters) : base(errorMessage, code, parameters)
        {
        }

        public override int HttpCode => 403;
    }
}
