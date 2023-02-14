namespace Codeed.Framework.Domain
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTime Timestamp { get; }
    }
}