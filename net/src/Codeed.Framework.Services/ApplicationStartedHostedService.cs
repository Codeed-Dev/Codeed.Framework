using Codeed.Framework.EventBus;
using Codeed.Framework.Services.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Codeed.Framework.Services
{
    internal class ApplicationStartedHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationStartedHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                await eventBus.Publish(new ApplicationStartedEvent());
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}