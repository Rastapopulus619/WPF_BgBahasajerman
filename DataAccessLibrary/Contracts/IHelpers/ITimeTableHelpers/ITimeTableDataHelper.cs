using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers
{
    public interface ITimeTableDataHelper
    {
        ObservableCollection<TimeTableRow> CloneTimeTableData(ObservableCollection<TimeTableRow> timeTableData);
        bool AreTimeTableDataEqual(ObservableCollection<TimeTableRow> original, ObservableCollection<TimeTableRow> updated);
        List<SlotEntry> GetDifferences(ObservableCollection<TimeTableRow> original, ObservableCollection<TimeTableRow> updated);
    }
}
