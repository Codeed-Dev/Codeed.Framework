using System;

namespace Codeed.Framework.Commons.Exceptions
{
    public class ServiceNotFoundException : Exception, IHttpException
    {
        public ServiceNotFoundException(string errorMessage) : base(errorMessage)
        {

        }

        public int HttpCode => 404;
    }
}
