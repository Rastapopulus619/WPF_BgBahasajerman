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
        private string _testValue = "Initial Value";
        public string TestValue
        {
            get => _testValue;
            set => SetProperty(ref _testValue, value, nameof(TestValue));
        }
        public BookedSlotsViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            serviceFactory.ConfigureServicesFor(this);

            SaveChangesCommand = new AsyncRelayCommand(ShowSavePrompt);
            RevertChangesCommand = new AsyncRelayCommand(ShowRevertPrompt);
            ConfirmSaveChangesCommand = new AsyncRelayCommand(InsertUpdatedSlots);
            ConfirmRevertChangesCommand = new AsyncRelayCommand(CancelUpdatedSlots);
            CancelSaveOrRevertCommand = new AsyncRelayCommand(CancelSaveOrRevert);

            FetchTimeTableData();
            FetchStudentList();
        }
        private async Task CancelSaveOrRevert()
        {
            return;
        }
        private async Task ShowSavePrompt()
        {
            PromptService.ShowPrompt(
                "Confirm Save",
                new TextBlock { Text = "Are you sure you want to save?" },
                ConfirmSaveChangesCommand,
                CancelSaveOrRevertCommand

            );
        }
        private async Task ShowRevertPrompt()
        {
            PromptService.ShowPrompt(
                "Confirm Revert",
                new TextBlock { Text = "Are you sure you want to revert?" },
                ConfirmRevertChangesCommand,
                CancelSaveOrRevertCommand
            );
        }
        private async Task InsertUpdatedSlots()
        {
            var differences = GetDifferences();

            // Check if differences is null or empty
            if (differences == null || !differences.Any())
            {
                MessageBox.Show("No changes detected. No action required.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            // Log the differences
            var changeDetails = GenerateChangeDetails(differences);

            bool userConfirmed = ConfirmAction(
                "Confirm Save",
                $"The following changes will be saved:\n\n{changeDetails}\n\nWould you like to proceed?"
            );

            if (!userConfirmed)
            {
                MessageBox.Show("Save operation canceled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Perform save operation
            await BookedSlotsDataService.SaveBookedSlotsAsync(differences);

            // Refresh timetable data
            FetchTimeTableData();
            MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private async Task CancelUpdatedSlots()
        {
            if (_timetableDataBackup == null)
            {
                MessageBox.Show("No previous state to revert to.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var differences = GetDifferences();
            var changeDetails = GenerateChangeDetails(differences, isReverting: true);

            bool userConfirmed = ConfirmAction(
                "Confirm Revert",
                $"The following changes will be reverted:\n\n{changeDetails}\n\nWould you like to proceed?"
            );

            if (!userConfirmed)
            {
                MessageBox.Show("Revert operation canceled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            RevertToSavedState();
            MessageBox.Show("Changes reverted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void SlotEntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanCancel = true;

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

                if(slotEntry.IsValid == false)
                {
                    CanSave = false;
                }
                else
                {
                    CanSave = NoInvalidValueExists();
                }
            }
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
                        $"{(isReverting ? "Reverting" : "Changing")} SlotID: {slot.SlotID}, " +
                        $"Day: {slot.WeekdayName}, Time: {slot.DayNumber}, " +
                        $"Name: {originalSlot.Name} -> {slot.Name}");
                }
            }

            return details.ToString();
        }
        private bool ConfirmAction(string title, string message)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            return result == MessageBoxResult.Yes;
        }

    }
}
