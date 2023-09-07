using Codeed.Framework.Domain;
using MediatR;

namespace Codeed.Framework.EventBus
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
    {
    }
}