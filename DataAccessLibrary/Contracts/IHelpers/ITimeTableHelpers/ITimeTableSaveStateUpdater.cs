using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers
{
    public interface ITimeTableSaveStateUpdater
    {
        void UpdateCanSaveAndCancel(
            ObservableCollection<TimeTableRow> timeTableDataBackup,
            ObservableCollection<TimeTableRow> timeTableData,
            out bool canSave,
            out bool canCancel);
        bool NoInvalidValueExists(ObservableCollection<TimeTableRow> timeTableData);
    }
}
