using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel
{
    public interface ISlotEntrySubscriptionManager
    {
        void SubscribeToSlotEntryChanges(ObservableCollection<TimeTableRow> timeTableData, PropertyChangedEventHandler handler);
        void UnsubscribeFromSlotEntryChanges(ObservableCollection<TimeTableRow> timeTableData, PropertyChangedEventHandler handler);
    }
}
