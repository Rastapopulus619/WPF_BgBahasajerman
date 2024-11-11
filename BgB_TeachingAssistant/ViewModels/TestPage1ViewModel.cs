using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using BgB_TeachingAssistant.Commands;
using Bgb_DataAccessLibrary.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class TestPage1ViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;

        public IDataServiceTestClass DataService { get; set; }
        public override string Name => "Test1";

        private string _studentID;
        private string _studentName;

        public string StudentID
        {
            get => _studentID;
            set
            {
                if (SetProperty(ref _studentID, value)) // Using SetProperty from ViewModelBase
                {
                    Console.WriteLine($"Message changed to: {_studentID}");
                    //GetStudentNameByID();
                    //***for instant change  uncomment this and add ``UpdateSourceTrigger=PropertyChanged`` into the XAML code
                }

            }
        }

        public string StudentName
        {
            get => _studentName;
            set
            {
                    if (SetProperty(ref _studentName, value)) // <<< how to use SetProperty from ViewModelBase
                    {
                        Console.WriteLine($"Message changed to: {_studentName}");
                    }
            }
        }

        public ICommand LookupCommand { get; }
        //public TestPage1ViewModel(IServiceFactory serviceFactory, IDataServiceTestClass dataService) : base(serviceFactory)
        public TestPage1ViewModel(IServiceFactory serviceFactory, IEventAggregator eventAggregator) : base(serviceFactory, eventAggregator)
        {
            // Configure necessary services for this view model     /////*********** USING REFLECTION?? WTF!!!!
            serviceFactory.ConfigureServicesFor(this);

            _eventAggregator = eventAggregator;

            // Subscribe to the event
            SubscribeToEvent<StudentNameByIDEvent>(OnStudentNameReceived);

            //DataService = dataService;
            Console.WriteLine("BreakPoint");

            LookupCommand = new AsyncRelayCommand(GetStudentNameByID);   ///in XAML:  <Button Content="Lookup" Command="{Binding LookupCommand}" Margin="0,5" />////
        }

        // Handle the event here
        private void OnStudentNameReceived(string studentName)
        {
            StudentName = studentName;
        }
        private void OnStudentNameReceived(StudentNameByIDEvent studentEvent)
        {
            StudentName = studentEvent.StudentName;
        }

        private async Task GetStudentNameByID()
        {
            Console.WriteLine($"Current Student ID: {StudentID}");

            if (int.TryParse(StudentID, out _))
            {
                // Call the data service, which will trigger the event
                await DataService.GetStudentNameByStudentID(StudentID);
            }
            else
            {
                StudentName = "Invalid ID";
            }
        }
    }
}
