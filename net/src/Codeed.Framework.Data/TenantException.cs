using System;
using System.Runtime.Serialization;

namespace Codeed.Framework.Data
{
    [Serializable]
    public class TenantException : Exception
    {
        public TenantException()
        {
        }

        public TenantException(string message) : base(message)
        {
        }

        public TenantException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TenantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}