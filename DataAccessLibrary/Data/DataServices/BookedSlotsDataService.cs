using System.Collections.ObjectModel;
using System.Data;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;
using Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventAggregators;
using Bgb_DataAccessLibrary.Contracts.IServices.IData;
using Bgb_DataAccessLibrary.Contracts.IDataAccess;
using Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryExecutor;
using Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryLoaders;
using System.Globalization;

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

            if (row == null)
            {
                // Default if no data is found
                return new SlotEntry
                {
                    Name = "-",
                    SlotID = slotNumber,
                    WeekdayName = "-",
                    Time = "-"
                };
            }

            return new SlotEntry
            {
                StudentID = row.IsNull("StudentID") ? 0 : row.Field<int>("StudentID"),
                Name = row.IsNull("StudentName") ? "-" : row.Field<string>("StudentName"),
                SlotID = row.Field<int>("SlotID"),
                Time = row.IsNull("Time") ? "-" : row.Field<string>("Time"),
                DayNumber = row.Field<int>("DayNumber"),
                WeekdayName = row.IsNull("WeekdayName") ? "-" : row.Field<string>("WeekdayName"),
                Level = row.IsNull("Level") ? null : row.Field<string>("Level"),
                Currency = row.IsNull("Currency") ? null : row.Field<string>("Currency"),
                Preis = row.IsNull("Preis") ? null : (decimal?)row.Field<decimal>("Preis"),
                PreisDisplayValue = GeneratePreisDisplayValue(
                    row.IsNull("Currency") ? null : row.Field<string>("Currency"),
                    row.IsNull("Preis") ? null : (decimal?)row.Field<decimal>("Preis"),
                    row.IsNull("DiscountAmount") ? null : (decimal?)row.Field<decimal>("DiscountAmount")),
                DiscountAmount = row.IsNull("DiscountAmount") ? null : (decimal?)row.Field<decimal>("DiscountAmount"),
                Content = row.IsNull("StudentName") ? "-" : row.Field<string>("StudentName")
            };
        }
        private string GeneratePreisDisplayValue(string? currency, decimal? preis, decimal? discountAmount)
        {
            if (currency == null || preis == null)
                return string.Empty;

            decimal calculatedPrice = preis.Value - (discountAmount ?? 0);

            // Create a custom NumberFormatInfo for grouping with dots and commas for decimals
            var customFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            customFormat.NumberGroupSeparator = ".";  // Dot for thousands separator
            customFormat.NumberDecimalSeparator = ","; // Comma for decimals

            // Format the calculated price with grouping and decimals
            string priceString = calculatedPrice.ToString("#,0.##", customFormat);

            return $"{currency} {priceString}";
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
