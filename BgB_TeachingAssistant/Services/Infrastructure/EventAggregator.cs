using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgB_TeachingAssistant.Services.Infrastructure
{
    public class EventAggregator
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> handler)
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions[typeof(T)] = new List<Delegate>();
            }
            _subscriptions[typeof(T)].Add(handler);
        }

        public void Publish<T>(T eventData)
        {
            if (_subscriptions.ContainsKey(typeof(T)))
            {
                foreach (var handler in _subscriptions[typeof(T)].Cast<Action<T>>())
                {
                    handler(eventData);
                }
            }
        }
    }

}
