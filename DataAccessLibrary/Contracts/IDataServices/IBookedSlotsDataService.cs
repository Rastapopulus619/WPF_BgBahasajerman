using Bgb_DataAccessLibrary.Models.StudentModels;
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
        /// <summary>
        /// Retrieves all booked slots and organizes them into a timetable structure.
        /// </summary>
        /// <returns>An ObservableCollection of TimeTableRow representing the timetable.</returns>
        Task<ObservableCollection<TimeTableRow>> GetBookedSlotsAsync();

        /// <summary>
        /// Saves a list of updated slot entries into the database.
        /// </summary>
        /// <param name="updatedSlots">The list of SlotEntry objects to be saved.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SaveBookedSlotsAsync(List<SlotEntry> updatedSlots);

        /// <summary>
        /// Retrieves the list of students from the database.
        /// </summary>
        /// <returns>A list of StudentModel objects.</returns>
        Task<List<StudentModel>> GetStudentsAsync();
    }
}
