using MVVM_UtilitiesLibrary.BaseClasses;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ViewModelBase : ObservableObject, IPageViewModel, IDisposable, IEventUnsubscriber, IViewModelBase
    {
        // Implement the Name property from IPageViewModel
        public virtual string Name { get; }
        public IServiceFactory ServiceFactory { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        public List<Delegate> EventHandlers { get; set; } = new List<Delegate>();
        public int EventHandlersCount => EventHandlers.Count;

        public ViewModelBase(IServiceFactory serviceFactory)
        {
            LogViewModelCreation();

            serviceFactory.ConfigureViewModelBaseServices(this);

            if (EventAggregator == null)
            {
                throw new InvalidOperationException("EventAggregator is not initialized.");
            }
        }
        public void LogViewModelCreation()
        {
            $"+[{GetType().Name}] "
                .ColorizeMulti(ConsoleColor.Blue)
                .Append($"Created at {DateTime.Now.ToString("HH:mm:ss")}, HashCode: ", ConsoleColor.DarkGray)
                .Append($"{this.GetHashCode()}", ConsoleColor.DarkYellow)
                .WriteLine();
        }


        // Subscribe to an event and track it for automatic unsubscription
        protected void SubscribeToEvent<T>(Action<T> handler)
        {
            EventAggregator.Subscribe(handler, this);
            //EventHandlers.Add(handler); // ******* this is done in the EventAggregator

            #region Old logging of an addition to the EventHandlers list
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
            #endregion
        }

        public void UnsubscribeEvents()
        {
            #region Unsubscription logging of EventHandlers
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
            #endregion

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

            // Unsubscribe all events to avoid memory leaks
            UnsubscribeEvents();

            // **** this seems unnecessary  // change this back if it is needed
            //EventAggregator.LogSubscriptions();  

            // Clear properties to free references
            ServiceFactory = null;
            EventAggregator = null;

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
        }
        // implement this Cleanup method in derived classes
        protected virtual void Cleanup()
        {
            // Derived classes can override this to perform additional cleanup tasks if needed.
        }
        ~ViewModelBase()
        {
            Console.WriteLine("ViewModelBase: Destructor called");
        }
    }
}
