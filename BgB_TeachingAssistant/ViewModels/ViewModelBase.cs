using System;
using System.Collections.Generic;
using Bgb_DataAccessLibrary.Contracts;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ViewModelBase : ObservableObject, IPageViewModel, IDisposable, IEventUnsubscriber, IViewModelBase
    {
        public IServiceFactory ServiceFactory { get; set; }
        public List<Delegate> EventHandlers { get; set; } = new List<Delegate>();
        public int EventHandlersCount => EventHandlers.Count;
        public IEventAggregator EventAggregator { get; set; }

        // Constructor for injecting the factory and event aggregator
        public ViewModelBase(IServiceFactory serviceFactory)
        {
            serviceFactory.ConfigureServicesFor(this);

            if (EventAggregator == null)
            {
                throw new InvalidOperationException("EventAggregator is not initialized.");
            }

            LogViewModelCreation();
            //ServiceFactory = serviceFactory;
            //EventAggregator = eventAggregator;
        }
        public void LogViewModelCreation()
        {
            $"+[{GetType().Name}] "
                .ColorizeMulti(ConsoleColor.Blue)
                .Append($"Created at {DateTime.Now.ToString("HH:mm:ss")}, HashCode: ", ConsoleColor.DarkGray)
                .Append($"{this.GetHashCode()}", ConsoleColor.DarkYellow)
                .WriteLine();
        }

        // Implement the Name property from IPageViewModel
        public virtual string Name { get; }

        // Subscribe to an event and track it for automatic unsubscription
        protected void SubscribeToEvent<T>(Action<T> handler)
        {

            //$"{this.GetType().Name}"
            //    .ColorizeMulti(ConsoleColor.DarkGreen)
            //    .Append(" (")
            //    .Append($"{this.GetHashCode()}", ConsoleColor.DarkYellow)
            //    .Append(") ")
            //    .Append("Subscribed", ConsoleColor.White,ConsoleColor.Green)
            //    .Append(" to event of type ", ConsoleColor.DarkGray, ConsoleColor.Black)
            //    .Append($"[{typeof(T).Name}]", ConsoleColor.DarkGreen)
            //    .Append(" with handler ", ConsoleColor.DarkGray)
            //    .Append($"{handler.Method.Name}()", ConsoleColor.DarkGreen)
            //    .Append(". Total handlers in EventHandlers: ", ConsoleColor.DarkGray)
            //    .Append($" {EventHandlers.Count} ", ConsoleColor.White, ConsoleColor.DarkGreen)
            //    .WriteLine();

            EventAggregator.Subscribe(handler, this);
            //EventHandlers.Add(handler); // ******* this is done in the EventAggregator

        }

        public void UnsubscribeEvents()
        {
            $"[{this.GetType().Name}]"
                .ColorizeMulti(ConsoleColor.Red)
                .Append(" (")
                .Append($"{this.GetHashCode()}", ConsoleColor.DarkYellow)
                .Append(") ")
                .Append("Unsubscribing", ConsoleColor.Black, ConsoleColor.DarkRed)
                .Append(" all... total amount: ", ConsoleColor.DarkGray)
                .Append($" {EventHandlers.Count} ", ConsoleColor.Black, ConsoleColor.DarkRed)
                .WriteLine();

            if (EventHandlers.Count == 0)
            {
                "No handlers".ColorizeMulti(ConsoleColor.Red).Append(" found in EventHandlers. Unsubscription skipped.", ConsoleColor.DarkGray).WriteLine();
                return;
            }

            //$"[{this.GetType().Name}]"
            //    .ColorizeMulti(ConsoleColor.Red)
            //    .Append(" (")
            //    .Append($"{this.GetHashCode()}", ConsoleColor.DarkYellow)
            //    .Append(") ")
            //    .Append("Unsubscribing", ConsoleColor.Black, ConsoleColor.Red)
            //    .Append(" all... total amount: ", ConsoleColor.DarkGray)
            //    .Append($" {EventHandlers.Count} ", ConsoleColor.Black, ConsoleColor.DarkRed)
            //    .WriteLine();

            var handlersToUnsubscribe = new List<Delegate>(EventHandlers);
            foreach (var handler in handlersToUnsubscribe)
            {
                try
                {
                    var handlerType = handler.GetType();
                    var eventType = handlerType.GetGenericArguments()[0];

                    Console.WriteLine($"Unsubscribing '{handler.Method.Name}' for event type '{eventType.Name}'");

                    var unsubscribeMethod = typeof(IEventAggregator)
                        .GetMethod(nameof(EventAggregator.Unsubscribe))
                        .MakeGenericMethod(eventType);

                    unsubscribeMethod.Invoke(EventAggregator, new object[] { handler });


                    Console.WriteLine($"Unsubscribed from event of type {eventType.Name} with handler {handler.Method.Name}");


                }
                catch (Exception ex)
                {
                    $"Error unsubscribing from event: {ex.Message}".ColorizeMulti(ConsoleColor.Red)
                        .Append($"[{ex.Message}]", ConsoleColor.DarkGreen)
                        .WriteLine(); ;
                }
            }

            // Clear the list to avoid further references
            EventHandlers.Clear();

            // Log the subscriptions after clearing
            EventAggregator.LogSubscriptions();
        }

        // IDisposable implementation for cleaning up subscriptions
        public void Dispose()
        {
            // Call to custom cleanup logic before unsubscribing events
            Cleanup();

            // **** Unsubscription from all events is now done in the Navigation Service // change this back if it sucks
            // UnsubscribeEvents();

            // Suppress finalization
            GC.SuppressFinalize(this);

            // Log disposal
            $"-[{GetType().Name}] "
                .ColorizeMulti(ConsoleColor.Red)
                .Append(" (")
                .Append($"{this.GetHashCode()}", ConsoleColor.DarkYellow)
                .Append(") ")
                .Append($"Successfully Disposed and Unsubscribed at {DateTime.Now.ToString("HH:mm:ss")}", ConsoleColor.DarkGray)
                .WriteLine();

            // **** this seems unnecessary  // change this back if it is needed
            //EventAggregator.LogSubscriptions();  
        }
        // Optional Cleanup method for derived classes
        protected virtual void Cleanup()
        {
            // Derived classes can override this to perform additional cleanup tasks if needed.
        }
    }
}
