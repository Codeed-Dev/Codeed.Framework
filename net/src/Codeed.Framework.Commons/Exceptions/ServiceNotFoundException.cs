using System;

namespace Codeed.Framework.Commons.Exceptions
{
    public class ServiceNotFoundException : BaseServiceException
    {
        public ServiceNotFoundException()
        {

        }

        public ServiceNotFoundException(string errorMessage) : base(errorMessage)
        {

        }

        public ServiceNotFoundException(string errorMessage, string code) : base(errorMessage, code)
        {
        }

        public ServiceNotFoundException(string errorMessage, string code, object parameters) : base(errorMessage, code, parameters)
        {
        }

        public override int HttpCode => 404;
    }
}
