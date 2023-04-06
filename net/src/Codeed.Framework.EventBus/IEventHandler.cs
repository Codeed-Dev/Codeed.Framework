using Codeed.Framework.Domain;

namespace Codeed.Framework.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}