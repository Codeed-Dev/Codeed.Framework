using MediatR;
using System;

namespace Codeed.Framework.Domain
{
    public abstract class Request<T> : Message, IRequest<T>
    {
        public DateTime Timestamp { get; private set; }

        protected Request()
        {
            Timestamp = DateTime.Now;
        }
    }
}