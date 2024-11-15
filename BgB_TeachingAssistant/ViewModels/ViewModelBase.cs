using Bgb_DataAccessLibrary.Models.Interfaces;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using System;
using System.Collections.Generic;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ViewModelBase : ObservableObject, IPageViewModel, IDisposable, IEventUnsubscriber
    {
        protected readonly IServiceFactory ServiceFactory;
        private readonly List<Delegate> _eventHandlers = new List<Delegate>();
        private readonly IEventAggregator _eventAggregator;

        // Constructor for injecting the factory and event aggregator
        public ViewModelBase(IServiceFactory serviceFactory, IEventAggregator eventAggregator)
        {
            ServiceFactory = serviceFactory;
            _eventAggregator = eventAggregator;
        }

        // Implement the Name property from IPageViewModel
        public virtual string Name { get; }

        // Subscribe to an event and track it for automatic unsubscription
        protected void SubscribeToEvent<T>(Action<T> handler)
        {
            _eventAggregator.Subscribe(handler);
            _eventHandlers.Add(handler);

            Console.WriteLine($"[{this.GetHashCode()}] Subscribed to event of type {typeof(T).Name} with handler {handler.Method.Name}. Total handlers in _eventHandlers: {_eventHandlers.Count}");
        }

        public void UnsubscribeEvents()
        {
            Console.WriteLine($"[{this.GetHashCode()}] Unsubscribing from all events... Handlers available for unsubscription: {_eventHandlers.Count}");

            if (_eventHandlers.Count == 0)
            {
                Console.WriteLine("No handlers found in _eventHandlers. Unsubscription skipped.");
                return;
            }

            _eventAggregator.LogSubscriptions();

            var handlersToUnsubscribe = new List<Delegate>(_eventHandlers);
            foreach (var handler in handlersToUnsubscribe)
            {
                try
                {
                    var handlerType = handler.GetType();
                    var eventType = handlerType.GetGenericArguments()[0];

                    Console.WriteLine($"Attempting to unsubscribe handler '{handler.Method.Name}' for event type '{eventType.Name}'");

                    var unsubscribeMethod = typeof(IEventAggregator)
                        .GetMethod(nameof(_eventAggregator.Unsubscribe))
                        .MakeGenericMethod(eventType);

                    unsubscribeMethod.Invoke(_eventAggregator, new object[] { handler });

                    Console.WriteLine($"Unsubscribed from event of type {eventType.Name} with handler {handler.Method.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error unsubscribing from event: {ex.Message}");
                }
            }

            // Clear the list to avoid further references
            _eventHandlers.Clear();

            // Log the subscriptions after clearing
            _eventAggregator.LogSubscriptions();
        }

        // IDisposable implementation for cleaning up subscriptions
        public void Dispose()
        {
            // Call to custom cleanup logic before unsubscribing events
            Cleanup();

            // Dispose will automatically unsubscribe from all events
            UnsubscribeEvents();

            // Suppress finalization
            GC.SuppressFinalize(this);

            // Log disposal
            Console.WriteLine($"{this.GetType().Name} disposed and unsubscribed from all events.");
        }
        // Optional Cleanup method for derived classes
        protected virtual void Cleanup()
        {
            // Derived classes can override this to perform additional cleanup tasks if needed.
        }
    }
}
