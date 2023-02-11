using Codeed.Framework.Domain;

namespace Codeed.Framework.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }

        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>()
           where T : Event
           where TH : IEventHandler<T>;

        void RemoveSubscription<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;

        bool HasSubscriptionsForEvent<T>() where T : Event;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() 
            where T : Event;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();

        string GetEventKey<T>(T @event)
            where T : Event;
    }
}
