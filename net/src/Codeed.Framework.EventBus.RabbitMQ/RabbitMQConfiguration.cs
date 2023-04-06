namespace Codeed.Framework.EventBus.RabbitMQ
{
    public class RabbitMQConfiguration
    {
        public string HostName { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string BrokerName { get; set; } = "";

        public string QueueName { get; set; } = "";
    }
}
