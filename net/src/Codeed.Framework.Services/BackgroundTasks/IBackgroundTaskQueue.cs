using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.Services.BackgroundTasks
{
    public interface IBackgroundTaskQueue
    {
        void EnqueueTask(Func<IServiceScopeFactory, CancellationToken, Task> task);

        Task<Func<IServiceScopeFactory, CancellationToken, Task>> DequeueTask(CancellationToken cancellationToken);
    }
}
