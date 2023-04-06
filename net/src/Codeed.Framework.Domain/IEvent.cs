namespace Codeed.Framework.Domain
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTimeOffset Timestamp { get; }
    }
}