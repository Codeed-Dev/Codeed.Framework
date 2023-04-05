using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Codeed.Framework.Services.BackgroundTasks
{
    public class BackgroundQueueHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BackgroundQueueHostedService> _logger;

        public BackgroundQueueHostedService(
            IBackgroundTaskQueue backgroundTaskQueue,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<BackgroundQueueHostedService> logger)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var task = await _backgroundTaskQueue.DequeueTask(cancellationToken);

                if (task is null)
                {
                    await Task.Delay(1000);
                }
                else
                {
                    try
                    {
                        await task(_serviceScopeFactory, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occured during execution of a background task");
                    }
                }
            }
        }
    }
}
