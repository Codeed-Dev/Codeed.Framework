using System;

namespace Codeed.Framework.Commons.Exceptions
{
    public class ServiceException : BaseServiceException
    {
        public ServiceException(string errorMessage) : base(errorMessage)
        {
        }

        public ServiceException(string errorMessage, string code) : base(errorMessage, code)
        {
        }

        public ServiceException(string errorMessage, string code, object parameters) : base(errorMessage, code, parameters)
        {
        }

        public override int HttpCode => 400;
    }
}
