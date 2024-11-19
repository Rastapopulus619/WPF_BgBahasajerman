namespace Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> handler)
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions[typeof(T)] = new List<Delegate>();
            }

            _subscriptions[typeof(T)].Add(handler);
            Console.WriteLine($"Subscribed to event of type {typeof(T).Name} with handler {handler.Method.Name}. Total subscriptions: {_subscriptions[typeof(T)].Count}");
        }
        
        public void Publish<T>(T eventData)
        {
            if (_subscriptions.ContainsKey(typeof(T)))
            {
                Console.WriteLine($"Publishing event of type {typeof(T).Name} with data: {eventData}");
                foreach (var handler in _subscriptions[typeof(T)].Cast<Action<T>>())
                {
                    handler(eventData);
                }
            }
        }

        // Unsubscribe method to remove the event handler
        public void Unsubscribe<T>(Action<T> handler)
        {
            if (_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions[typeof(T)].Remove(handler);
                Console.WriteLine($"Unsubscribed from event of type {typeof(T).Name} with handler {handler.Method.Name}. Total subscriptions: {_subscriptions[typeof(T)].Count}");

                // Clean up if no more handlers are left for this event type
                if (_subscriptions[typeof(T)].Count == 0)
                {
                    _subscriptions.Remove(typeof(T));
                    Console.WriteLine($"No more handlers left for event type {typeof(T).Name}. Removing event type from subscriptions.");
                }
            }
            else
            {
                Console.WriteLine($"Tried to unsubscribe from event of type {typeof(T).Name} but no handlers were found.");
            }
        }

        // For logging current subscriptions before and after dispose/unsubscribe
        public void LogSubscriptions()
        {
            Console.WriteLine("Current Subscriptions:");
            foreach (var eventType in _subscriptions)
            {
                Console.WriteLine($"Event Type: {eventType.Key.Name}, Handlers Count: {eventType.Value.Count}");
            }
        }

    }
}
