using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Services
{

    public interface IServiceCore
    {
    }

    public interface IService : IServiceCore
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }

    public interface IService<TRet> : IServiceCore
    {
        Task<TRet> ExecuteAsync(CancellationToken cancellationToken);
    }

    public interface IService<T1, TRet> : IServiceCore
    {
        Task<TRet> ExecuteAsync(T1 param1, CancellationToken cancellationToken);
    }

    public interface IService<T1, T2, TRet> : IServiceCore
    {
        Task<TRet> ExecuteAsync(T1 param1, T2 param2, CancellationToken cancellationToken);
    }

    public interface IService<T1, T2, T3, TRet> : IServiceCore
    {
        Task<TRet> ExecuteAsync(T1 param1, T2 param2, T3 param3, CancellationToken cancellationToken);
    }

    public interface IServiceWithoutResponse<T1> : IServiceCore
    {
        Task ExecuteAsync(T1 param1, CancellationToken cancellationToken);
    }

    public interface IServiceWithoutResponse<T1, T2> : IServiceCore
    {
        Task ExecuteAsync(T1 param1, T2 param2, CancellationToken cancellationToken);
    }

    public interface IServiceWithoutResponse<T1, T2, T3> : IServiceCore
    {
        Task ExecuteAsync(T1 param1, T2 param2, T3 param3, CancellationToken cancellationToken);
    }
}
