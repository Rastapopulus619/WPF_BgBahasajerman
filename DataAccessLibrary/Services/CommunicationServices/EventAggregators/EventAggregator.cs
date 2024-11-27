using Bgb_DataAccessLibrary.Models.Interfaces;
using System;
using static Bgb_DataAccessLibrary.Helpers.ExtensionMethods.StringExtensionMethods;

namespace Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> handler, IPageViewModel viewModel)
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions[typeof(T)] = new List<Delegate>();
            }

            _subscriptions[typeof(T)].Add(handler);
            viewModel.EventHandlers.Add(handler);

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

            // Cast viewModel to ViewModelBase to access EventHandlersCount
            if (viewModel is IPageViewModel viewModelBase)
            {
                "Total subscriptions in _subscriptions: ".ColorizeMulti(ConsoleColor.DarkGray)
                    .Append($" {_subscriptions[typeof(T)].Count} ", ConsoleColor.Black, ConsoleColor.DarkGreen)
                    .Append($". Total eventHandlers in [{viewModel.GetType().ToString().Substring(viewModel.GetType().ToString().LastIndexOf('.') + 1)}]'s _eventHandlers: ", ConsoleColor.DarkGray)
                    .Append($" {viewModelBase.EventHandlersCount} ", ConsoleColor.Black, ConsoleColor.DarkGreen)
                    .WriteLine();
            }
            else
            {
                // Handle the case where viewModel is not a ViewModelBase
                "Total subscriptions in _subscriptions: ".ColorizeMulti(ConsoleColor.DarkGray)
                    .Append($" {_subscriptions[typeof(T)].Count} ", ConsoleColor.Black, ConsoleColor.DarkGreen)
                    .Append(". Unable to retrieve _eventHandlers count.", ConsoleColor.DarkRed)
                    .WriteLine();
            }

            LogSubscriptions();
        }
        public void Subscribe<T>(Action<T> handler)
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions[typeof(T)] = new List<Delegate>();
            }

            _subscriptions[typeof(T)].Add(handler);

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
                .Append(". Total subscriptions in _subscriptions: ", ConsoleColor.DarkGray)
                .Append($" {_subscriptions[typeof(T)].Count} ", ConsoleColor.Black, ConsoleColor.DarkGreen)
                .WriteLine();

            LogSubscriptions();
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
            var eventType = typeof(T);

            if (_subscriptions.ContainsKey(eventType))
            {
                _subscriptions[eventType].Remove(handler);

                $"Unsubscribing handler {handler.Method.Name}() from event of type ".ColorizeMulti(ConsoleColor.DarkGray).Append($"{eventType.Name}.",ConsoleColor.Black, ConsoleColor.DarkGray).WriteLine();   
                    ;

                // Log current subscriptions for the specific event type
                LogSubscriptions(eventType);

                // Clean up if no more handlers are left for this event type
                if (_subscriptions[eventType].Count == 0)
                {
                    _subscriptions.Remove(eventType);

                    $"No more handlers left for event type {eventType.Name}. Removing event type from subscriptions."
                        .Colorize(ConsoleColor.DarkGray);
                }
            }
            else
            {
                $"Tried to unsubscribe from event of type {eventType.Name}, but no handlers were found!"
                    .Colorize(ConsoleColor.Red);
            }
        }

        // For logging current subscriptions before and after dispose/unsubscribe
        public void LogSubscriptions()
        {
            ">> Current Subscriptions:".Colorize(ConsoleColor.DarkGray);

            foreach (var eventType in _subscriptions)
            {
                $"Event Type: ".ColorizeMulti(ConsoleColor.DarkGray).Append($"[{eventType.Key.Name}]",ConsoleColor.Black, ConsoleColor.DarkGray).Append(", Handlers Count: ", ConsoleColor.DarkGray).Append($" {eventType.Value.Count} ", ConsoleColor.Black, ConsoleColor.DarkGray).WriteLine();

                foreach (var handler in eventType.Value)
                {
                    var methodInfo = handler.Method;
                    var declaringType = methodInfo.DeclaringType;
                    var target = handler.Target;
                    string targetInfo;

                    if (target != null)
                    {
                        var targetHashCode = target.GetHashCode();
                        targetInfo = $"{targetHashCode}";
                    }
                    else
                    {
                        targetInfo = "Static Method";
                    }

                    //Console.WriteLine($" -  : {declaringType?.Name}.{methodInfo.Name} ({targetInfo})");
                    $"          - ({targetInfo}) {declaringType?.Name}.{methodInfo.Name}()".Colorize(ConsoleColor.DarkGray);
                }
            }
        }
        // Overloaded method to log subscriptions of a specific eventType
        public void LogSubscriptions(Type eventType)
        {
            if (_subscriptions.ContainsKey(eventType))
            {
                $"Event type: ".ColorizeMulti(ConsoleColor.DarkGray)
                    .Append($"[{eventType.Name}]", ConsoleColor.Black, ConsoleColor.DarkGray)
                    .Append(". Total subscriptions in _subscriptions:: ", ConsoleColor.DarkGray)
                    .Append($" {_subscriptions[eventType].Count} ", ConsoleColor.Black, ConsoleColor.DarkGreen)
                    .WriteLine();

                foreach (var handler in _subscriptions[eventType])
                {
                    var methodInfo = handler.Method;
                    var declaringType = methodInfo.DeclaringType;
                    var target = handler.Target;
                    string targetInfo;

                    if (target != null)
                    {
                        var targetHashCode = target.GetHashCode();
                        targetInfo = $"{targetHashCode}";
                    }
                    else
                    {
                        targetInfo = "Static Method";
                    }

                    $"          - ({targetInfo}) {declaringType?.Name}.{methodInfo.Name}()".Colorize(ConsoleColor.DarkGray);
                }
            }
            else
            {
                $"Remove event type ".ColorizeMulti(ConsoleColor.DarkGray)
                     .Append($"[{eventType.Name}]..", ConsoleColor.Black, ConsoleColor.DarkGray)
                     .WriteLine();
            }
        }



    }
}
