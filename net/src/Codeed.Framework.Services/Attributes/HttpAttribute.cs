using System;

namespace Codeed.Framework.Services.Attributes
{
    public class HttpServiceAttribute : Attribute
    {
        public HttpServiceAttribute(HttpServiceMethod method, string route)
        {
            Route = route;
            Method = method;
        }

        public string Route { get; }
        public HttpServiceMethod Method { get; }
    }

}
