﻿using BgB_TeachingAssistant.Commands;
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
        public override string Name => "Test1";

        private string _studentID;
        public string StudentID
        {
            get => _studentID;
            set
            {
                if (SetProperty(ref _studentID, value)) // ◀️◀️◀️ Using SetProperty from ViewModelBase
                {
                    #region Explanation of SetProperty Parameters

                    /*
                    Let's break down each line:

                    - ref T field: This parameter is the backing field (like _studentName) for the property being set. 
                      The ref keyword allows SetProperty to directly modify the field in the calling property.

                    - T value: This is the new value being assigned to the property. It will be compared to the current value of field.

                    - [CallerMemberName] string propertyName = null: This attribute is used to automatically pass the 
                      name of the calling property (in this case, StudentName) as the propertyName parameter. 
                      If you change the property name, this still works without needing updates to the string in SetProperty, 
                      reducing error-proneness.
                    */

                    #endregion

                    Console.WriteLine($"Message changed to: {_studentID}");
                    // Validate the input to check if it's a positive integer
                    if (int.TryParse(value, out int result) && result > 0)
                    {
                        IsLookupButtonEnabled = true; // Enable the button
                    }
                    else
                    {
                        IsLookupButtonEnabled = false; // Disable the button
                    }
                    //GetStudentNameByID();
                    //***for instant change  uncomment this and add ``UpdateSourceTrigger=PropertyChanged`` into the XAML code
                }
            }
        }
        private string _studentName;
        public string StudentName
        {
            get => _studentName;
            set
            {
                    if (SetProperty(ref _studentName, value)) // ◀️◀️◀️ Using SetProperty from ViewModelBase
                    {
                        Console.WriteLine($"Message changed to: {_studentName}");
                    }
            }
        }
        private bool _isLookupButtonEnabled;
        public bool IsLookupButtonEnabled
        {
            get => _isLookupButtonEnabled;
            set
            {
                if (SetProperty(ref _isLookupButtonEnabled, value)) // Notify changes to the property
                {
                    // Optional: Log or take other actions when the button state changes
                }
            }
        }


        public IDataServiceTestClass DataService { get; set; }
        public IStudentNameByIDEvent StudentNameByIDEvent { get; set; }
        public ICommand LookupCommand { get; }

        public TestPage1ViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            IsLookupButtonEnabled = false; // Disable the button initially

            // Configure necessary services for this view model 🔻🔻🔻    ////******* USING REFLECTION!! WTF!!!! ******
            serviceFactory.ConfigureServicesFor(this);

            // Subscribe to the event 🔻🔻🔻
            //**********SubscribeToEvents THROUGH ViewModelBase! not _eventAggregator
            SubscribeToEvent<StudentNameByIDEvent>(OnStudentNameReceived);

            LookupCommand = new AsyncRelayCommand(GetStudentNameByID);   ///in XAML:  <Button Content="Lookup" Command="{Binding LookupCommand}" Margin="0,5" />////
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
        // Eventhandling method 🔻🔻🔻
        private void OnStudentNameReceived(StudentNameByIDEvent studentEvent)
        {
            StudentName = studentEvent.StudentName;
        }
        protected override void Cleanup()
        {
            // Unsubscribe from events
            UnsubscribeEvents();

            // Nullify properties to free up memory
            DataService = null;
            StudentNameByIDEvent = null;

            // Call base class cleanup
            base.Cleanup();
        }
    }
}
