using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Services
{
    public static class Service
    {
        public static class WithParameters<T1>
        {
            public abstract class WithResponse<TRet> : IService<T1, TRet>
            {
                public abstract Task<TRet> ExecuteAsync(T1 param1, CancellationToken cancellationToken);
            }

            public abstract class WithoutResponse : IServiceWithoutResponse<T1>
            {
                public abstract Task ExecuteAsync(T1 param1, CancellationToken cancellationToken);
            }
        }

        public static class WithParameters<T1, T2>
        {
            public abstract class WithResponse<TRet> : IService<T1, T2, TRet>
            {
                public abstract Task<TRet> ExecuteAsync(T1 param1, T2 param2, CancellationToken cancellationToken);
            }

            public abstract class WithoutResponse : IServiceWithoutResponse<T1, T2>
            {
                public abstract Task ExecuteAsync(T1 param1, T2 param2, CancellationToken cancellationToken);
            }
        }

        public static class WithParameters<T1, T2, T3>
        {
            public abstract class WithResponse<TRet> : IService<T1, T2, T3, TRet>
            {
                public abstract Task<TRet> ExecuteAsync(T1 param1, T2 param2, T3 param3, CancellationToken cancellationToken);
            }

            public abstract class WithoutResponse : IServiceWithoutResponse<T1, T2, T3>
            {
                public abstract Task ExecuteAsync(T1 param1, T2 param2, T3 param3, CancellationToken cancellationToken);
            }
        }

        public static class WithoutParameters
        {
            public abstract class WithResponse<TRet> : IService<TRet>
            {
                public abstract Task<TRet> ExecuteAsync(CancellationToken cancellationToken);
            }

            public abstract class WithoutResponse : IService
            {
                public abstract Task ExecuteAsync(CancellationToken cancellationToken);
            }
        }
    }
}
