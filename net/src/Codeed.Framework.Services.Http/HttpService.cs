using Codeed.Framework.AspNet;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Services
{
    public static class HttpService
    {
        public static class WithParameters<T1>
        {
            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithResponse<TRet> : BaseController, IService<T1, TRet>
            {
                public abstract Task<TRet> ExecuteAsync(T1 param1, CancellationToken cancellationToken);
            }

            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithoutResponse : BaseController, IServiceWithoutResponse<T1>
            {
                public abstract Task ExecuteAsync(T1 param1, CancellationToken cancellationToken);
            }
        }

        public static class WithParameters<T1, T2>
        {
            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithResponse<TRet> : BaseController, IService<T1, T2, TRet>
            {
                public abstract Task<TRet> ExecuteAsync(T1 param1, T2 param2, CancellationToken cancellationToken);
            }

            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithoutResponse : BaseController, IServiceWithoutResponse<T1, T2>
            {
                public abstract Task ExecuteAsync(T1 param1, T2 param2, CancellationToken cancellationToken);
            }
        }

        public static class WithParameters<T1, T2, T3>
        {
            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithResponse<TRet> : BaseController, IService<T1, T2, T3, TRet>
            {
                public abstract Task<TRet> ExecuteAsync(T1 param1, T2 param2, T3 param3, CancellationToken cancellationToken);
            }

            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithoutResponse : BaseController, IServiceWithoutResponse<T1, T2, T3>
            {
                public abstract Task ExecuteAsync(T1 param1, T2 param2, T3 param3, CancellationToken cancellationToken);
            }
        }

        public static class WithoutParameters
        {
            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithResponse<TRet> : BaseController, IService<TRet>
            {
                public abstract Task<TRet> ExecuteAsync(CancellationToken cancellationToken);
            }

            [ApiExplorerSettings(GroupName = "Services")]
            public abstract class WithoutResponse : BaseController, IService
            {
                public abstract Task ExecuteAsync(CancellationToken cancellationToken);
            }
        }
    }
}
