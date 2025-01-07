using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventManagement
{
    public interface IPropertyChangeSubscriptionService
    {
        /// <summary>
        /// Subscribes to changes in the specified nested properties of the given objects.
        /// </summary>
        /// <typeparam name="T">The type of the parent objects.</typeparam>
        /// <param name="items">The collection of parent objects to monitor.</param>
        /// <param name="handler">The handler to invoke when a nested property changes.</param>
        /// <param name="propertySelector">A function that selects the nested properties to monitor.</param>
        void SubscribeToNestedProperties<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler,
            Func<T, IEnumerable<INotifyPropertyChanged>> propertySelector);

        /// <summary>
        /// Unsubscribes from changes in the specified nested properties of the given objects.
        /// </summary>
        /// <typeparam name="T">The type of the parent objects.</typeparam>
        /// <param name="items">The collection of parent objects to stop monitoring.</param>
        /// <param name="handler">The handler to remove from nested properties.</param>
        /// <param name="propertySelector">A function that selects the nested properties to stop monitoring.</param>
        void UnsubscribeFromNestedProperties<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler,
            Func<T, IEnumerable<INotifyPropertyChanged>> propertySelector);

        /// <summary>
        /// Subscribes to changes in the flat observable objects themselves.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <param name="items">The collection of observable objects to monitor.</param>
        /// <param name="handler">The handler to invoke when a property changes.</param>
        void SubscribeToFlatObjects<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler)
            where T : INotifyPropertyChanged;

        /// <summary>
        /// Unsubscribes from changes in the flat observable objects themselves.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <param name="items">The collection of observable objects to stop monitoring.</param>
        /// <param name="handler">The handler to remove from the objects.</param>
        void UnsubscribeFromFlatObjects<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler)
            where T : INotifyPropertyChanged;
    }
}
