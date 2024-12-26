using Bgb_DataAccessLibrary.Contracts;
using System.Collections.ObjectModel;
using System.Data;
using Bgb_SharedLibrary.DTOs.TimeTableDTOs;
using Bgb_DataAccessLibrary.Models.StudentModels;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public class BookedSlotsDataService : IBookedSlotsDataService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        private readonly IQueryExecutor _queryExecutor;
        private readonly IEventAggregator _eventAggregator;


        public BookedSlotsDataService(IDataAccess dataAccess, IQueryLoader queryLoader, IQueryExecutor queryExecutor, IEventAggregator eventAggregator)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            _queryExecutor = queryExecutor;
            _eventAggregator = eventAggregator;
        }

        public async Task<ObservableCollection<TimeTableRow>> GetBookedSlotsAsync()
        {
            DataTable dt = await _queryExecutor.ExecuteQueryAsDataTableAsync("GetAllBookedSlots");

            var timeSlots = await _queryExecutor.ExecuteQueryAsync<string>("GetAllLessonTimeSpanStrings");

            //var timeSlots = await _queryExecutor.ExecuteQueryAsync("GetAllLessonTimeSpanStrings");
            // Example: Time slots for rows
            //var timeSlots = new[] { "07:00-08:30", "08:30-10:00", "10:00-11:30", "13:00-14:30", "14:30-16:00", "16:00-17:30", "17:30-19:00", "19:00-20:30" };

            List<TimeTableRow> timetableRows = new List<TimeTableRow>();

                //int additionValue = 7;
                
            for ( int i = 0; i < timeSlots.Count; i++)
            {
                TimeTableRow row = new TimeTableRow
                {
                    Zeiten = timeSlots[i],
                    Montag = GetDayEntryForDayTheRightWay(dt, i + 1),
                    Dienstag = GetDayEntryForDayTheRightWay(dt, i + 9),
                    Mittwoch = GetDayEntryForDayTheRightWay(dt, i + 17),
                    Donnerstag = GetDayEntryForDayTheRightWay(dt, i + 25),
                    Freitag = GetDayEntryForDayTheRightWay(dt, i + 33),
                    Samstag = GetDayEntryForDayTheRightWay(dt, i + 41),
                    Sonntag = GetDayEntryForDayTheRightWay(dt, i + 49)
                };

                timetableRows.Add(row);
            }

            return new ObservableCollection<TimeTableRow>(timetableRows);
        }
        private SlotEntry GetDayEntryForDayTheRightWay(DataTable dt, int slotNumber)
        {
            var row = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("SlotID") == slotNumber);

            if (row == null) return new SlotEntry { Name = "-", SlotID = slotNumber}; // Default if no data

            return new SlotEntry
            {
                StudentID = row.Field<int>("StudentID"),
                Name = row.Field<string>("Name"),
                SlotID = row.Field<int>("SlotID"),
                DayNumber = row.Field<int>("DayNumber"),
                WeekdayName = row.Field<string>("WeekdayName"),
                Level = row.IsNull("Level") ? null : row.Field<string>("Level"),
                Currency = row.IsNull("Currency") ? null : row.Field<string>("Currency"),
                Preis = row.IsNull("Preis") ? null : (decimal?)row.Field<decimal>("Preis"),
                DiscountAmount = row.IsNull("DiscountAmount") ? null : (decimal?)row.Field<int>("DiscountAmount"),
                Content = row.Field<string>("Name")
            };
        }
        public async Task SaveBookedSlotsAsync(List<SlotEntry> updatedSlots)
        {
            if (updatedSlots == null || !updatedSlots.Any())
            {
                Console.WriteLine("No updates to save.");
                return;
            }

            foreach (var slot in updatedSlots)
            {
                try
                {
                    // Step 1: Delete existing row with the same SlotID
                    await _queryExecutor.ExecuteCommandAsync("DeleteBySlotID", new { SlotID = slot.SlotID });

                    if(slot.Name == "-")
                    {
                        Console.WriteLine($"Successfully deleted SlotID: {slot.SlotID}");
                        continue;
                    }
                    // Step 2: Insert new row with SlotID and StudentID
                    await _queryExecutor.ExecuteCommandAsync("InsertNewSlotBooking", new { SlotID = slot.SlotID, StudentID = slot.StudentID });

                    Console.WriteLine($"Successfully updated SlotID: {slot.SlotID}, StudentID: {slot.StudentID}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating SlotEntry SlotID: {slot.SlotID}, Error: {ex.Message}");
                }
            }
        }
        public async Task<List<StudentModel>> GetStudentsAsync()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            return (await _dataAccess.QueryAsync<StudentModel>(query)).ToList();
        }
    }
}
