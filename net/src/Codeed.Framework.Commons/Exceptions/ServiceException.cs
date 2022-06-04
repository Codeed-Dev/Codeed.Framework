using System;

namespace Influencer.Core.Exceptions
{
    public class ServiceException : Exception, IHttpException
    {
        public ServiceException(string errorMessage) : base(errorMessage)
        {

        }

        public int HttpCode => 400;
    }
}
