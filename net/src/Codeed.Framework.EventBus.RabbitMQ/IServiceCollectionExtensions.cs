using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Codeed.Framework.EventBus.RabbitMQ
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRabbitMqEventBus(this IServiceCollection services, RabbitMQConfiguration rabbitMQConfiguration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = rabbitMQConfiguration.HostName,
                    DispatchConsumersAsync = true,
                    UserName = rabbitMQConfiguration.Username,
                    Password = rabbitMQConfiguration.Password
                };

                return new DefaultRabbitMQPersistentConnection(factory, logger);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubscription = new InMemoryEventBusSubscriptionsManager();
                return new EventBusRabbitMQ(rabbitMQPersistentConnection,
                                            logger,
                                            services,
                                            new InMemoryEventBusSubscriptionsManager(),
                                            rabbitMQConfiguration.BrokerName,
                                            rabbitMQConfiguration.QueueName,
                                            5);

            });

            return services;
        }
    }
}
