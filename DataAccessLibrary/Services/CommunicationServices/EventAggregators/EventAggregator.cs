using System;
using System.Collections.Generic;
using System.Linq;
using Bgb_DataAccessLibrary.Contracts;
using Bgb_DataAccessLibrary.Helpers.ExtensionMethods;

namespace Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators
{
    public class EventAggregator : IEventAggregator
    {
        // Store WeakEventSubscription objects instead of strong Delegate references
        private readonly Dictionary<Type, List<WeakEventSubscription<object>>> _subscriptions = new Dictionary<Type, List<WeakEventSubscription<object>>>();

        // Subscribe method to add a weak event subscription
        public void Subscribe<T>(Action<T> handler, IPageViewModel viewModel)
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions[typeof(T)] = new List<WeakEventSubscription<object>>();
            }

            var weakSubscription = new WeakEventSubscription<object>(viewModel, (args) => handler((T)args));
            _subscriptions[typeof(T)].Add(weakSubscription);

            Console.WriteLine($"Created weak reference for handler {handler.Method.Name} of type {typeof(T).Name}");

            // Logging subscription details
            LogSubscriptionDetails(handler, viewModel);
        }

        // Log subscription details
        private void LogSubscriptionDetails<T>(Action<T> handler, IPageViewModel viewModel)
        {
            // Keeping your logging logic
            $"{handler.Method.DeclaringType?.Name.Substring(handler.Method.DeclaringType.Name.LastIndexOf('.') + 1)}"
                .ColorizeMulti(ConsoleColor.DarkGreen)
                .Append(" (")
                .Append($"{handler.Target?.GetHashCode()}", ConsoleColor.DarkYellow)
                .Append(") ")
                .Append("Subscribed", ConsoleColor.Black, ConsoleColor.DarkGreen)
                .Append(" to event of type ", ConsoleColor.DarkGray)
                .Append($"[{typeof(T).Name}]", ConsoleColor.DarkGreen)
                .Append(" with handler ", ConsoleColor.DarkGray)
                .Append($"{handler.Method.Name}()", ConsoleColor.DarkGreen)
                .WriteLine();

            LogSubscriptions();
        }

        // Publish method to invoke weak subscriptions
        public void Publish<T>(T eventData)
        {
            if (_subscriptions.ContainsKey(typeof(T)))
            {
                Console.WriteLine($"Publishing event of type {typeof(T).Name} with data: {eventData}");

                // Remove any dead subscriptions before publishing
                _subscriptions[typeof(T)].RemoveAll(sub =>
                {
                    if (!sub.IsAlive)
                    {
                        Console.WriteLine($"Weak reference for event type {typeof(T).Name} has been garbage collected.");
                        return true;
                    }
                    return false;
                });

                // Invoke all remaining subscribers
                foreach (var subscription in _subscriptions[typeof(T)])
                {
                    subscription.Invoke(this, eventData);
                    Console.WriteLine($"Invoked handler for event type {typeof(T).Name}");
                }
            }
        }

        // Unsubscribe method to remove a weak event subscription
        public void Unsubscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);

            if (_subscriptions.ContainsKey(eventType))
            {
                // Find and remove any subscriptions that match the handler
                _subscriptions[eventType].RemoveAll(sub =>
                {
                    if (sub.IsAlive && sub.HandlerMatches(handler))
                    {
                        Console.WriteLine($"Removing handler {handler.Method.Name} from event type {eventType.Name}");
                        return true;
                    }
                    else if (!sub.IsAlive)
                    {
                        Console.WriteLine($"Handler for event type {eventType.Name} was already collected by garbage collector.");
                    }
                    return false;
                });

                // Clean up if no more handlers are left for this event type
                if (_subscriptions[eventType].Count == 0)
                {
                    _subscriptions.Remove(eventType);
                    Console.WriteLine($"No more handlers left for event type {eventType.Name}. Removing event type from subscriptions.");
                }
            }
            else
            {
                Console.WriteLine($"Tried to unsubscribe from event of type {eventType.Name}, but no handlers were found!");
            }
        }

        // Log current subscriptions
        public void LogSubscriptions()
        {
            ">> Current Subscriptions:".Colorize(ConsoleColor.DarkGray);

            foreach (var eventType in _subscriptions)
            {
                $"Event Type: ".ColorizeMulti(ConsoleColor.DarkGray).Append($"[{eventType.Key.Name}]", ConsoleColor.Black, ConsoleColor.DarkGray).Append(", Handlers Count: ", ConsoleColor.DarkGray).Append($" {eventType.Value.Count} ", ConsoleColor.Black, ConsoleColor.DarkGray).WriteLine();

                foreach (var subscription in eventType.Value)
                {
                    var methodInfo = subscription.MethodInfo;
                    var declaringType = methodInfo?.DeclaringType;
                    var target = subscription._targetRef.Target; // Access the public field directly

                    string targetInfo = target != null ? $"{target.GetHashCode()}" : "Static Method";
                    $"          - ({targetInfo}) {declaringType?.Name}.{methodInfo?.Name}()".Colorize(ConsoleColor.DarkGray);
                }
            }
        }
    }
}
