using MediatR;

namespace Codeed.Framework.Domain
{
    public interface IEvent : INotification
    {
        Guid Id { get; }

        DateTimeOffset Timestamp { get; }
    }
}