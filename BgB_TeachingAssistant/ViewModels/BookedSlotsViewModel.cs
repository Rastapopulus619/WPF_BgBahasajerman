using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_SharedLibrary.DTOs.TimeTableDTOs;
using BgB_TeachingAssistant.Commands;
using BgB_TeachingAssistant.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            set => SetProperty(ref _students, value, nameof(Students));  // Provide the property name
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
        public ICommand SaveChangesCommand { get; }
        public ICommand RevertChangesCommand { get; }
        public ICommand ConfirmSaveChangesCommand { get; }
        public ICommand ConfirmRevertChangesCommand { get; }
        public ICommand CancelSaveOrRevertCommand { get; }
        public ICommand ToggleContentVisibilityCommand { get; }
        public BookedSlotsViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            serviceFactory.ConfigureServicesFor(this);

            SaveChangesCommand = new AsyncRelayCommand(ShowSavePrompt);
            RevertChangesCommand = new AsyncRelayCommand(ShowRevertPrompt);
            ToggleContentVisibilityCommand = new RelayCommand(_ => IsContentVisible = !IsContentVisible);

            FetchTimeTableData();
            FetchStudentList();
        }
        private async Task ShowSavePrompt()
        {
            var differences = GetDifferences();
            var changeDetails = GenerateChangeDetails(differences);

            bool userChoice = PromptService.ShowOkCancelPrompt(
                "Confirm Save",
                $"Are you sure you want to save these Changes?\n\n{changeDetails}");

            if (userChoice == true)
            {
                // Perform save operation
                await BookedSlotsDataService.SaveBookedSlotsAsync(differences);

                FetchTimeTableData();
                PromptService.ShowInformationPrompt("Success", $"These Changes:\n\n{changeDetails}\nhave been saved.");
            }
            else
            {
                return;
            }
        }
        private async Task ShowRevertPrompt()
        {
            bool userChoice = PromptService.ShowOkCancelPrompt(
                "Confirm Revert",
                "Are you sure you want to revert Changes?");

            if (userChoice == true)
            {
                var differences = GetDifferences();
                var changeDetails = GenerateChangeDetails(differences);

                RevertToSavedState();
                PromptService.ShowInformationPrompt("Success", $"These Changes have been reverted.:\n\n{changeDetails}");
            }
            else
            {
                return;
            }
        }
        private async void FetchTimeTableData()
        {
            try
            {
                TimetableData = await BookedSlotsDataService.GetBookedSlotsAsync();

                // Save the state after successfully fetching the data
                SaveState();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching booked slots: {ex.Message}");
            }
        }
        private async void FetchStudentList()
        {
            try
            {
                await LoadStudentsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving Student Objects: {ex.Message}");
            }
        }
        private async Task LoadStudentsAsync()
        {
            // Retrieve students using the data service
            var students = await BookedSlotsDataService.GetStudentsAsync();
            Students = new ObservableCollection<StudentModel>(students);
        }
        public void SaveState()
        {
            _timetableDataBackup = CloneTimetableData(TimetableData);
            UpdateCanSaveAndCancel();
        }
        public void RevertToSavedState()
        {
            if (_timetableDataBackup != null)
            {
                TimetableData = CloneTimetableData(_timetableDataBackup);
                UpdateCanSaveAndCancel();
            }
        }
        public void UpdateCanSaveAndCancel()
        {

            CanCancel = !AreTimetableDataEqual();

            if(CanCancel == false)
            {
                CanSave = false;
                return;
            }
            else
            {
                CanSave = NoInvalidValueExists();
            }
        }
        public bool AreTimetableDataEqual()
        {
            // Null checks for both collections
            if (_timetableDataBackup == null || TimetableData == null)
                return false;

            // Check if counts are different
            if (_timetableDataBackup.Count != TimetableData.Count)
                return false;

            // Compare each row and its SlotEntries
            for (int i = 0; i < TimetableData.Count; i++)
            {
                var currentRow = TimetableData[i];
                var backupRow = _timetableDataBackup[i];

                if (!AreTimeTableRowsEqual(currentRow, backupRow))
                {
                    return false;
                }
            }

            return true; // All rows and their child objects are equal
        }
        private bool AreTimeTableRowsEqual(TimeTableRow currentRow, TimeTableRow backupRow)
        {
            if (currentRow == null || backupRow == null)
                return false;

            // Compare each day's SlotEntry
            return AreSlotEntriesEqual(currentRow.Montag, backupRow.Montag) &&
                   AreSlotEntriesEqual(currentRow.Dienstag, backupRow.Dienstag) &&
                   AreSlotEntriesEqual(currentRow.Mittwoch, backupRow.Mittwoch) &&
                   AreSlotEntriesEqual(currentRow.Donnerstag, backupRow.Donnerstag) &&
                   AreSlotEntriesEqual(currentRow.Freitag, backupRow.Freitag) &&
                   AreSlotEntriesEqual(currentRow.Samstag, backupRow.Samstag) &&
                   AreSlotEntriesEqual(currentRow.Sonntag, backupRow.Sonntag);
        }
        private bool AreSlotEntriesEqual(SlotEntry current, SlotEntry backup)
        {
            if (current == null || backup == null)
                return false;

            // Compare properties of SlotEntry
            return current.StudentID == backup.StudentID &&
                   current.Name == backup.Name &&
                   current.SlotID == backup.SlotID &&
                   current.Time == backup.Time &&
                   current.DayNumber == backup.DayNumber &&
                   current.WeekdayName == backup.WeekdayName &&
                   current.Level == backup.Level &&
                   current.Currency == backup.Currency &&
                   current.Preis == backup.Preis &&
                   current.DiscountAmount == backup.DiscountAmount &&
                   current.Content == backup.Content &&
                   current.IsEditable == backup.IsEditable &&
                   current.IsValid == backup.IsValid &&
                   current.Comments == backup.Comments;
        }
        private ObservableCollection<TimeTableRow> CloneTimetableData(ObservableCollection<TimeTableRow> original)
        {
            if (original == null) return null;

            var clone = new ObservableCollection<TimeTableRow>();
            foreach (var row in original)
            {
                var clonedRow = new TimeTableRow
                {
                    Zeiten = row.Zeiten,
                    Montag = CloneSlotEntry(row.Montag),
                    Dienstag = CloneSlotEntry(row.Dienstag),
                    Mittwoch = CloneSlotEntry(row.Mittwoch),
                    Donnerstag = CloneSlotEntry(row.Donnerstag),
                    Freitag = CloneSlotEntry(row.Freitag),
                    Samstag = CloneSlotEntry(row.Samstag),
                    Sonntag = CloneSlotEntry(row.Sonntag),
                };

                clone.Add(clonedRow);
            }

            return clone;
        }
        private SlotEntry CloneSlotEntry(SlotEntry original)
        {
            if (original == null) return null;

            return new SlotEntry
            {
                StudentID = original.StudentID,
                Name = original.Name,
                SlotID = original.SlotID,
                Time = original.Time,
                DayNumber = original.DayNumber,
                WeekdayName = original.WeekdayName,
                Level = original.Level,
                Currency = original.Currency,
                Preis = original.Preis,
                DiscountAmount = original.DiscountAmount,
                Content = original.Content,
                IsEditable = original.IsEditable,
                IsValid = original.IsValid,
                Comments = original.Comments
            };
        }
        public List<SlotEntry> GetDifferences()
        {
            var differences = new List<SlotEntry>();

            if (TimetableData == null || _timetableDataBackup == null)
            {
                throw new InvalidOperationException("TimetableData or backup data is null.");
            }

            for (int i = 0; i < TimetableData.Count; i++)
            {
                var currentRow = TimetableData[i];
                var backupRow = _timetableDataBackup[i];

                // Compare each day's SlotEntry in the row
                CompareSlotEntries(differences, currentRow.Montag, backupRow.Montag);
                CompareSlotEntries(differences, currentRow.Dienstag, backupRow.Dienstag);
                CompareSlotEntries(differences, currentRow.Mittwoch, backupRow.Mittwoch);
                CompareSlotEntries(differences, currentRow.Donnerstag, backupRow.Donnerstag);
                CompareSlotEntries(differences, currentRow.Freitag, backupRow.Freitag);
                CompareSlotEntries(differences, currentRow.Samstag, backupRow.Samstag);
                CompareSlotEntries(differences, currentRow.Sonntag, backupRow.Sonntag);
            }

            return differences;
        }
        private void CompareSlotEntries(List<SlotEntry> differences, SlotEntry current, SlotEntry backup)
        {
            if (current == null || backup == null) return;

            // Compare properties and add to differences if any mismatch is found
            if (!current.Equals(backup))
            {
                differences.Add(current);
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

                bool timeTablesEqual = AreTimetableDataEqual();

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
