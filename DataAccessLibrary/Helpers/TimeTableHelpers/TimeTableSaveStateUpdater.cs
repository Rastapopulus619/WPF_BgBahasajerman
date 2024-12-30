using Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Helpers.TimeTableHelpers
{
    public class TimeTableSaveStateUpdater : ITimeTableSaveStateUpdater
    {
        private readonly ITimeTableDataHelper _timeTableDataHelper;
        public TimeTableSaveStateUpdater(ITimeTableDataHelper timeTableDataHelper)
        {
            _timeTableDataHelper = timeTableDataHelper;
        }
        public void UpdateCanSaveAndCancel(
            ObservableCollection<TimeTableRow> timetableDataBackup,
            ObservableCollection<TimeTableRow> timetableData,
            out bool canSave,
            out bool canCancel)
        {
            canCancel = !_timeTableDataHelper.AreTimetableDataEqual(timetableDataBackup, timetableData);
            canSave = canCancel && timetableData != null && !timetableData.Any(row => !IsRowValid(row));
        }

        private bool IsRowValid(TimeTableRow row)
        {
            return IsSlotEntryValid(row.Montag) &&
                   IsSlotEntryValid(row.Dienstag) &&
                   IsSlotEntryValid(row.Mittwoch) &&
                   IsSlotEntryValid(row.Donnerstag) &&
                   IsSlotEntryValid(row.Freitag) &&
                   IsSlotEntryValid(row.Samstag) &&
                   IsSlotEntryValid(row.Sonntag);
        }

        private bool IsSlotEntryValid(SlotEntry slotEntry)
        {
            return slotEntry == null || slotEntry.IsValid;
        }
        public bool NoInvalidValueExists(ObservableCollection<TimeTableRow> timetableData)
        {
            if (timetableData == null)
                return true;

            return timetableData.All(IsRowValid);
        }
    }
}
