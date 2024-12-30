using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace BgB_TeachingAssistant.Services.BookedSlotsViewModel
{
    public class SlotEntrySubscriptionManager : ISlotEntrySubscriptionManager
    {
            public void SubscribeToSlotEntryChanges(ObservableCollection<TimeTableRow> timetableData, PropertyChangedEventHandler handler)
            {
                if (timetableData == null) throw new ArgumentNullException(nameof(timetableData));

                foreach (var row in timetableData)
                {
                    SubscribeToRow(row, handler);
                }
            }

            public void UnsubscribeFromSlotEntryChanges(ObservableCollection<TimeTableRow> timetableData, PropertyChangedEventHandler handler)
            {
                if (timetableData == null) return;

                foreach (var row in timetableData)
                {
                    UnsubscribeFromRow(row, handler);
                }
            }

            private void SubscribeToRow(TimeTableRow row, PropertyChangedEventHandler handler)
            {
                row.Montag.PropertyChanged += handler;
                row.Dienstag.PropertyChanged += handler;
                row.Mittwoch.PropertyChanged += handler;
                row.Donnerstag.PropertyChanged += handler;
                row.Freitag.PropertyChanged += handler;
                row.Samstag.PropertyChanged += handler;
                row.Sonntag.PropertyChanged += handler;
            }

            private void UnsubscribeFromRow(TimeTableRow row, PropertyChangedEventHandler handler)
            {
                row.Montag.PropertyChanged -= handler;
                row.Dienstag.PropertyChanged -= handler;
                row.Mittwoch.PropertyChanged -= handler;
                row.Donnerstag.PropertyChanged -= handler;
                row.Freitag.PropertyChanged -= handler;
                row.Samstag.PropertyChanged -= handler;
                row.Sonntag.PropertyChanged -= handler;
            }
        }

    }