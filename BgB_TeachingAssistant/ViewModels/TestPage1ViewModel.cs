using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Factories;
using BgB_TeachingAssistant.Commands;
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
        public IDataServiceTestClass DataService;
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
                    LookupStudentName();
                }
                
            }
        }

        public string StudentName
        {
            get => _studentName;
            set
            {
                    if (SetProperty(ref _studentName, value)) // Using SetProperty from ViewModelBase
                    {
                        Console.WriteLine($"Message changed to: {_studentName}");
                    }
            }
        }

        public ICommand LookupCommand { get; }
        public TestPage1ViewModel(IServiceFactory serviceFactory, IDataServiceTestClass dataService) : base(serviceFactory)
        {
            DataService = dataService;
            LookupCommand = new AsyncRelayCommand(LookupStudentName);
        }
        private async Task LookupStudentName()
        {
            Console.WriteLine($"Current Student ID: {StudentID}");

            // Simulate a lookup. Replace with actual logic to retrieve student name by ID.
            if (int.TryParse(StudentID, out int id))
            {
                // For demonstration, let's say the student name for ID 1 is "John Doe"
                // Replace this logic with a call to your data access layer or service.
                StudentName = id == 1 ? "John Doe" : "Student Not Found";
                
            }
            else
            {
                string name = await DataService.ProcessDataGetSingle();

                StudentName = $"{name}";
                //StudentName = "Invalid ID";
                Console.WriteLine($"Newly Stored StudentName is: {StudentName}");
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
