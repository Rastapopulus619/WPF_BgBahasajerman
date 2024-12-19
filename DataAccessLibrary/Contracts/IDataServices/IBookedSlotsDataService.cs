using Bgb_SharedLibrary.DTOs.TimeTableDTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IBookedSlotsDataService
    {
        Task<ObservableCollection<TimeTableRow>> GetBookedSlotsAsync();
    }
}
