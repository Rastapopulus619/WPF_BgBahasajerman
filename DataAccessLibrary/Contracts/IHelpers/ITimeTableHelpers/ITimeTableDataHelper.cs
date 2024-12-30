using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers
{
    public interface ITimeTableDataHelper
    {
        ObservableCollection<TimeTableRow> CloneTimetableData(ObservableCollection<TimeTableRow> timetableData);
        bool AreTimetableDataEqual(ObservableCollection<TimeTableRow> original, ObservableCollection<TimeTableRow> updated);
        List<SlotEntry> GetDifferences(ObservableCollection<TimeTableRow> original, ObservableCollection<TimeTableRow> updated);
    }
}
