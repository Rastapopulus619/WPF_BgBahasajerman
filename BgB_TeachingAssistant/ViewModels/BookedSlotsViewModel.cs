using System.Collections.ObjectModel;
using Bgb_SharedLibrary.DTOs.TimeTableDTOs;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary;
using BgB_TeachingAssistant.Services;
using System.Data;
using System.Security.AccessControl;
using Bgb_DataAccessLibrary.Events;
using System.Formats.Tar;

namespace BgB_TeachingAssistant.ViewModels
{
    class BookedSlotsViewModel : ViewModelBase
    {
        public override string Name => "BookedSlots";

        private ObservableCollection<TimeTableRow> _timetableData;
        public ObservableCollection<TimeTableRow> TimetableData
        {
                get => _timetableData;
                set => SetProperty(ref _timetableData, value, nameof(TimetableData));
        }
        public IDataServiceTestClass DataService { get; set; }
        public IBookedSlotsDataService BookedSlotsDataService { get; set; }
        public DataTable Dt { get; set; }
        public BookedSlotsViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            serviceFactory.ConfigureServicesFor(this);

            FetchTimeTableData();

            
        }
        private async void FetchTimeTableData()
        {
            try
            {
                // Fetch the timetable data
                TimetableData = await BookedSlotsDataService.GetBookedSlotsAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as logging
                Console.WriteLine($"Error fetching booked slots: {ex.Message}");
            }

            //Console.WriteLine($"Sunday last Slot Content is: {TimetableData[7].Sonntag.Content}");
        }

        protected override void Cleanup()
        {
            // Unsubscribe from events
            UnsubscribeEvents();

            // Clear TimetableData to break bindings
            if (TimetableData != null)
            {
                TimetableData.Clear();
                TimetableData = null; // Nullify to break binding
            }

            // Call base class cleanup
            base.Cleanup();
        }
    }
}
