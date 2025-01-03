﻿using Bgb_DataAccessLibrary.Models.Domain.StudentModels;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;
using BgB_TeachingAssistant.Commands;
using BgB_TeachingAssistant.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers;
using Bgb_DataAccessLibrary.Contracts.IServices.IDialog;

namespace BgB_TeachingAssistant.ViewModels
{
    class BookedSlotsViewModel : ViewModelBase
    {
        public override string Name => "BookedSlots";
        private bool _isDisposed = false;
        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value, nameof(CanSave));
        }
        private bool _canCancel;
        public bool CanCancel
        {
            get => _canCancel;
            set => SetProperty(ref _canCancel, value, nameof(CanCancel));
        }
        private string _testValue = "Initial Value";
        public string TestValue
        {
            get => _testValue;
            set => SetProperty(ref _testValue, value, nameof(TestValue));
        }

        private bool _isContentVisible = true;
        public bool IsContentVisible
        {
            get => _isContentVisible;
            set => SetProperty(ref _isContentVisible, value, nameof(IsContentVisible));
        }

        private ObservableCollection<StudentModel> _students;
        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value, nameof(Students));
        }
        private ObservableCollection<TimeTableRow> _timetableDataBackup;
        private ObservableCollection<TimeTableRow> _timetableData;
        public ObservableCollection<TimeTableRow> TimetableData
        {
                get => _timetableData;
            set
            {
                if (SetProperty(ref _timetableData, value, nameof(TimetableData)))
                {
                    UnsubscribeFromSlotEntryChanges(); // Clean up old subscriptions
                    if (!_isDisposed && _timetableData != null)
                    {
                        SubscribeToSlotEntryChanges(); // Subscribe to new data
                    }
                }
            }

        }
        public IBookedSlotsDataService BookedSlotsDataService { get; set; }
        public IPromptService PromptService { get; set; }
        public ITimeTableDataHelper TimeTableDataHelper { get; set; }
        public ICommand SaveChangesCommand { get; }
        public ICommand RevertChangesCommand { get; }
        public ICommand ToggleContentVisibilityCommand { get; }
        public BookedSlotsViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            serviceFactory.ConfigureServicesFor(this);

            SaveChangesCommand = new AsyncRelayCommand(ShowSavePrompt);
            RevertChangesCommand = new RelayCommand(ShowRevertPrompt); // no async operations, so use RelayCommand
            ToggleContentVisibilityCommand = new RelayCommand(_ => IsContentVisible = !IsContentVisible);

            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            try
            {
                // Run both async tasks in parallel
                var fetchTimeTableTask = FetchTimeTableDataAsync();
                var fetchStudentListTask = FetchStudentListAsync();

                await Task.WhenAll(fetchTimeTableTask, fetchStudentListTask);

                // Optionally log or handle post-initialization logic here
                Console.WriteLine("Initialization complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }
        private async Task FetchTimeTableDataAsync()
        {
            try
            {
                TimetableData = await BookedSlotsDataService.GetBookedSlotsAsync();
                SaveState();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching booked slots: {ex.Message}");
            }
        }
        private async Task FetchStudentListAsync()
        {
            try
            {
                Students = new ObservableCollection<StudentModel>(await BookedSlotsDataService.GetStudentsAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving Student Objects: {ex.Message}");
            }
        }

        #region Prompt Logic
        private async Task ShowSavePrompt()
        {
            var differences = TimeTableDataHelper.GetDifferences(_timetableDataBackup, TimetableData);
            var changeDetails = GenerateChangeDetails(differences);

            bool userChoice = PromptService.ShowOkCancelPrompt(
                "Confirm Save",
                $"Are you sure you want to save these Changes?\n\n{changeDetails}");

            if (userChoice == true)
            {
                // Perform save operation
                await BookedSlotsDataService.SaveBookedSlotsAsync(differences);

                await FetchTimeTableDataAsync();
                PromptService.ShowInformationPrompt("Success", $"These Changes:\n\n{changeDetails}\nhave been saved.");
            }
            else
            {
                return;
            }
        }
        private void ShowRevertPrompt()
        {
            bool userChoice = PromptService.ShowOkCancelPrompt(
                "Confirm Revert",
                "Are you sure you want to revert Changes?");

            if (userChoice == true)
            {
                var differences = TimeTableDataHelper.GetDifferences(_timetableDataBackup, TimetableData);
                var changeDetails = GenerateChangeDetails(differences);

                RevertToSavedState();
                PromptService.ShowInformationPrompt("Success", $"These Changes have been reverted.:\n\n{changeDetails}");
            }
            else
            {
                return;
            }
        }
        #endregion
        public void SaveState()
        {
            _timetableDataBackup = TimeTableDataHelper.CloneTimetableData(TimetableData);
            UpdateCanSaveAndCancel();
        }
        public void RevertToSavedState()
        {
            if (_timetableDataBackup != null)
            {
                TimetableData = TimeTableDataHelper.CloneTimetableData(_timetableDataBackup);
                UpdateCanSaveAndCancel();
            }
        }
        public void UpdateCanSaveAndCancel()
        {

            CanCancel = !TimeTableDataHelper.AreTimetableDataEqual(_timetableDataBackup, TimetableData);

            if (CanCancel == false)
            {
                CanSave = false;
                return;
            }
            else
            {
                CanSave = NoInvalidValueExists();
            }
        }
        private string GenerateChangeDetails(List<SlotEntry> changes, bool isReverting = false)
        {
            var details = new StringBuilder();

            foreach (var slot in changes)
            {
                var originalSlot = _timetableDataBackup
                    .SelectMany(row => new[] { row.Montag, row.Dienstag, row.Mittwoch, row.Donnerstag, row.Freitag, row.Samstag, row.Sonntag })
                    .FirstOrDefault(s => s?.SlotID == slot.SlotID);

                if (originalSlot != null)
                {
                    details.AppendLine(
                        $"{slot.WeekdayName}, {slot.Time}:\t[{originalSlot.Name}]    ->    [{slot.Name}]");
                }
            }
            return details.ToString();
        }
        private bool NoInvalidValueExists()
        {
            if (TimetableData == null)
                return true; // If there's no data, consider it valid (adjust as per your logic)

            foreach (var row in TimetableData)
            {
                // Check all SlotEntries in the row
                if (!IsSlotEntryValid(row.Montag) || !IsSlotEntryValid(row.Dienstag) ||
                    !IsSlotEntryValid(row.Mittwoch) || !IsSlotEntryValid(row.Donnerstag) ||
                    !IsSlotEntryValid(row.Freitag) || !IsSlotEntryValid(row.Samstag) ||
                    !IsSlotEntryValid(row.Sonntag))
                {
                    return false; // Return false immediately if any SlotEntry is invalid
                }
            }

            return true; // All SlotEntries are valid
        }
        // Logic to subscribe so that input is handled in real time
        private void SlotEntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SlotEntry.Name) && sender is SlotEntry slotEntry)
            {
                // Check if Name is empty or null
                if (string.IsNullOrWhiteSpace(slotEntry.Name))
                {
                    // Automatically set Name to "-"
                    slotEntry.Name = "-";
                }

                // Find the matching student in the Students collection
                var matchingStudent = Students?.FirstOrDefault(student => student.Name == slotEntry.Name);

                if (slotEntry.Name == "-")
                {
                    // If Name is "-", allow it and set IsValid to true
                    slotEntry.IsValid = true;
                    slotEntry.StudentID = 0;
                }
                else if (matchingStudent != null)
                {
                    // Match found in the Students collection
                    slotEntry.IsValid = true;
                    slotEntry.StudentID = matchingStudent.StudentID;
                }
                else
                {
                    // No match found
                    slotEntry.IsValid = false;
                }

                // Optional: Add logging or additional actions
                Console.WriteLine($"SlotEntry.Name changed: {slotEntry.Name}, IsValid: {slotEntry.IsValid}, StudentID: {slotEntry.StudentID}");

                bool timeTablesEqual = TimeTableDataHelper.AreTimetableDataEqual(_timetableDataBackup, TimetableData);

                if (timeTablesEqual == false)
                {
                    if (slotEntry.IsValid == false)
                    {
                        //Console.WriteLine("Slot Entry Invalid! CanSave = false - CanCancel = true");
                        CanSave = false;
                        CanCancel = true;
                    }
                    else if (slotEntry.IsValid == true)
                    {
                        CanCancel = true;
                        CanSave = NoInvalidValueExists();
                        //if (CanSave == true)
                        //{
                        //    Console.WriteLine("No Invalid Value detected! CanCancel = true - CanSave = true"); ;
                        //}
                    }
                }
                else
                {
                    CanCancel = false;
                    CanSave = false;
                }
            }
        }
        private bool IsSlotEntryValid(SlotEntry slotEntry)
        {
            return slotEntry == null || slotEntry.IsValid; // Null entries are considered valid
        }
        private void SubscribeToSlotEntryChanges()
        {
            if (TimetableData == null)
            { 
                throw new NullReferenceException("TimetableData cannot be null when subscribing to SlotEntry changes.");
            }

            foreach (var row in TimetableData)
            {
                SubscribeToRow(row);
            }
        }
        private void SubscribeToRow(TimeTableRow row)
        {
            row.Montag.PropertyChanged += SlotEntryPropertyChanged;
            row.Dienstag.PropertyChanged += SlotEntryPropertyChanged;
            row.Mittwoch.PropertyChanged += SlotEntryPropertyChanged;
            row.Donnerstag.PropertyChanged += SlotEntryPropertyChanged;
            row.Freitag.PropertyChanged += SlotEntryPropertyChanged;
            row.Samstag.PropertyChanged += SlotEntryPropertyChanged;
            row.Sonntag.PropertyChanged += SlotEntryPropertyChanged;
        }
        // Unsubscribe and Cleanup
        private void UnsubscribeFromSlotEntryChanges()
        {
            if (_timetableData == null) return;

            foreach (var row in _timetableData)
            {
                UnsubscribeFromRow(row);
            }
        }
        private void UnsubscribeFromRow(TimeTableRow row)
        {
            row.Montag.PropertyChanged -= SlotEntryPropertyChanged;
            row.Dienstag.PropertyChanged -= SlotEntryPropertyChanged;
            row.Mittwoch.PropertyChanged -= SlotEntryPropertyChanged;
            row.Donnerstag.PropertyChanged -= SlotEntryPropertyChanged;
            row.Freitag.PropertyChanged -= SlotEntryPropertyChanged;
            row.Samstag.PropertyChanged -= SlotEntryPropertyChanged;
            row.Sonntag.PropertyChanged -= SlotEntryPropertyChanged;
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

            _isDisposed = true; // Set the disposed flag

            // Call base class cleanup
            base.Cleanup();
        }
    }
}
