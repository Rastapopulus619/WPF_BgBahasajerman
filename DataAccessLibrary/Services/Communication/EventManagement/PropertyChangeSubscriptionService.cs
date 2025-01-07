using Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bgb_DataAccessLibrary.Services.Communication.EventManagement
{
    public class PropertyChangeSubscriptionService : IPropertyChangeSubscriptionService
    {
        /// <summary>
        /// Subscribes to changes in the specified nested properties of the given objects.
        /// </summary>
        public void SubscribeToNestedProperties<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler,
            Func<T, IEnumerable<INotifyPropertyChanged>> propertySelector)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

            foreach (var item in items)
            {
                foreach (var property in propertySelector(item))
                {
                    property.PropertyChanged += handler;
                }
            }
        }

        /// <summary>
        /// Unsubscribes from changes in the specified nested properties of the given objects.
        /// </summary>
        public void UnsubscribeFromNestedProperties<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler,
            Func<T, IEnumerable<INotifyPropertyChanged>> propertySelector)
        {
            if (items == null) return;
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

            foreach (var item in items)
            {
                foreach (var property in propertySelector(item))
                {
                    property.PropertyChanged -= handler;
                }
            }
        }

        /// <summary>
        /// Subscribes to changes in flat observable objects themselves.
        /// </summary>
        public void SubscribeToFlatObjects<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler)
            where T : INotifyPropertyChanged
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            foreach (var item in items)
            {
                item.PropertyChanged += handler;
            }
        }

        /// <summary>
        /// Unsubscribes from changes in flat observable objects themselves.
        /// </summary>
        public void UnsubscribeFromFlatObjects<T>(
            IEnumerable<T> items,
            PropertyChangedEventHandler handler)
            where T : INotifyPropertyChanged
        {
            if (items == null) return;
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            foreach (var item in items)
            {
                item.PropertyChanged -= handler;
            }
        }

        #region Handler Examples

        // Example handler for flat objects
        private void FlatObjectPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var changedObject = sender as INotifyPropertyChanged;

            Console.WriteLine($"Property '{e.PropertyName}' of object {changedObject} changed.");

            // Example conditional action
            if (e.PropertyName == "CityOfResidence")
            {
                Console.WriteLine("CityOfResidence was updated.");
            }
        }

        // Example handler for nested properties
        private void NestedObjectPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var nestedObject = sender as INotifyPropertyChanged;

            Console.WriteLine($"Nested object property '{e.PropertyName}' changed.");

            // Example conditional action
            if (e.PropertyName == "SomeNestedProperty")
            {
                Console.WriteLine("A nested property was updated.");
            }
        }

        #endregion

        #region Usage Examples

        // Example for subscribing to flat objects
        // ObservableCollection<StudentManagerDisplayStudentModel> students = new ObservableCollection<StudentManagerDisplayStudentModel>();
        // var service = new PropertyChangeSubscriptionService();
        // service.SubscribeToFlatObjects(
        //     students,
        //     FlatObjectPropertyChangedHandler);

        // Example for unsubscribing from flat objects
        // service.UnsubscribeFromFlatObjects(
        //     students,
        //     FlatObjectPropertyChangedHandler);

        // Example for subscribing to complex objects with nested properties
        // ObservableCollection<TimeTableRow> timetableRows = new ObservableCollection<TimeTableRow>();
        // var service = new PropertyChangeSubscriptionService();
        // service.SubscribeToNestedProperties(
        //     timetableRows,
        //     NestedObjectPropertyChangedHandler,
        //     row => new List<INotifyPropertyChanged>
        //     {
        //         row.Montag, row.Dienstag, row.Mittwoch,
        //         row.Donnerstag, row.Freitag, row.Samstag, row.Sonntag
        //     });

        // Example for unsubscribing from nested properties
        // service.UnsubscribeFromNestedProperties(
        //     timetableRows,
        //     NestedObjectPropertyChangedHandler,
        //     row => new List<INotifyPropertyChanged>
        //     {
        //         row.Montag, row.Dienstag, row.Mittwoch,
        //         row.Donnerstag, row.Freitag, row.Samstag, row.Sonntag
        //     });

        #endregion
    }
}
