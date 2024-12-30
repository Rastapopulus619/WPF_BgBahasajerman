using Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers;
using Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.ITimeTableDTOs;
using Bgb_DataAccessLibrary.Contracts.IModels.IStudentModels;
using Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel;
using Bgb_DataAccessLibrary.Contracts.IServices.IData;
using Bgb_DataAccessLibrary.Contracts.IServices.IDialog;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;
using BgB_TeachingAssistant.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    class BookedSlotsViewModel : ViewModelBase
    {
        public override string Name => "BookedSlots";
        private bool _isDisposed = false;

        #region ViewStateProperties
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
        #endregion

        //continue here: how to use extended xaml techniques here
        private bool _isContentVisible = true;
        public bool IsContentVisible
        {
            get => _isContentVisible;
            set => SetProperty(ref _isContentVisible, value, nameof(IsContentVisible));
        }
        private Style _currentCellStyle;
        public Style CurrentCellStyle
        {
            get => _currentCellStyle;
            set => SetProperty(ref _currentCellStyle, value);
        }

        public Style DefaultCellStyle { get; set; }
        public Style AlternateCellStyle { get; set; }


        private string _testValue = "Initial Value";
        public string TestValue
        {
            get => _testValue;
            set => SetProperty(ref _testValue, value, nameof(TestValue));
        }
        #region ViewDataProperties
        private ObservableCollection<IStudentModel> _students;
        public ObservableCollection<IStudentModel> Students
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
                    // UnsubscribeFromSlotEntryChanges(); // Clean up old subscriptions
                    SubscriptionManager.UnsubscribeFromSlotEntryChanges(_timetableData, SlotEntryPropertyChanged);
                    if (!_isDisposed && _timetableData != null)
                    {
                        //SubscribeToSlotEntryChanges(); // Subscribe to new data
                        SubscriptionManager.SubscribeToSlotEntryChanges(_timetableData, SlotEntryPropertyChanged); // Subscribe to new data
                    }
                }
            }
        }
        #endregion
        #region Dependencies
        #region IServices
        public IBookedSlotsInitializer Initializer {get; set; }
        public IBookedSlotsDataService BookedSlotsDataService { get; set; }
        public IPromptService PromptService { get; set; }
        public IBookedSlotsPromptHandler BookedSlotsPromptHandler { get; set; }
        public ISlotEntrySubscriptionManager SubscriptionManager { get; set; }
        #endregion
        #region IHelpers
        public ITimeTableDataHelper TimeTableDataHelper { get; set; }
        public ISlotEntryValidator SlotEntryValidator { get; set; }
        public ITimeTableSaveStateUpdater TimeTableSaveStateUpdater { get; set; }
        #endregion
        #region ICommands
        public ICommand SaveChangesCommand { get; }
        public ICommand RevertChangesCommand { get; }
        public ICommand ToggleContentVisibilityCommand { get; }
        public ICommand ToggleCellStyleCommand { get; }
        #endregion
        #endregion
        public BookedSlotsViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            

            serviceFactory.ConfigureServicesFor(this);

            SaveChangesCommand = new AsyncRelayCommand(ShowSavePrompt);
            RevertChangesCommand = new RelayCommand(ShowRevertPrompt); // no async operations, so use RelayCommand
            ToggleContentVisibilityCommand = new RelayCommand(_ => IsContentVisible = !IsContentVisible);

            // Initialize toggle style command (without LoadStyle logic here)
            ToggleCellStyleCommand = new RelayCommand(_ =>
            {
                CurrentCellStyle = CurrentCellStyle == DefaultCellStyle
                    ? AlternateCellStyle
                    : DefaultCellStyle;
            });

            InitializeAsync();
        }
        #region Initialization
        private async void InitializeAsync()
        {
            try
            {
                // Use the initializer to fetch data
                TimetableData = await Initializer.FetchTimeTableDataAsync();
                Students = await Initializer.FetchStudentListAsync();

                SaveState();
                Console.WriteLine("Initialization complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }
        #endregion
        #region Prompt Logic
        private async Task ShowSavePrompt()
        {
            var differences = TimeTableDataHelper.GetDifferences(_timetableDataBackup, TimetableData);

            if (BookedSlotsPromptHandler.SavePromptUserChoice(differences, _timetableDataBackup)) // Delegates to BookedSlotsPromptHandler
            {
                await BookedSlotsDataService.SaveBookedSlotsAsync(differences); // Save operation
                TimetableData = await Initializer.FetchTimeTableDataAsync(); // Refresh data
                SaveState();
                PromptService.ShowInformationPrompt("Success", $"These Changes:\n\n{BookedSlotsPromptHandler.Changes}\nhave been saved.");
            }
        }
        private void ShowRevertPrompt()
        {
            var differences = TimeTableDataHelper.GetDifferences(_timetableDataBackup, TimetableData);

            if (BookedSlotsPromptHandler.RevertPromptUserChoice(differences, _timetableDataBackup)) // Delegates to BookedSlotsPromptHandler
            {
                RevertToSavedState();
                PromptService.ShowInformationPrompt("Success", $"These Changes:\n\n{BookedSlotsPromptHandler.Changes}\nhave been saved.");
            }
        }
        #endregion
        #region State Logic
        public void SaveState()
        {
            _timetableDataBackup = TimeTableDataHelper.CloneTimetableData(TimetableData);

            // Use intermediate variables for out parameters
            TimeTableSaveStateUpdater.UpdateCanSaveAndCancel(
                _timetableDataBackup,
                TimetableData,
                out var canSave,
                out var canCancel);

            // Assign to properties
            CanSave = canSave;
            CanCancel = canCancel;
        }
        public void RevertToSavedState()
        {
            if (_timetableDataBackup != null)
            {
                TimetableData = TimeTableDataHelper.CloneTimetableData(_timetableDataBackup);

                // Use intermediate variables for out parameters
                TimeTableSaveStateUpdater.UpdateCanSaveAndCancel(
                    _timetableDataBackup,
                    TimetableData,
                    out var canSave,
                    out var canCancel);

                // Assign to properties
                CanSave = canSave;
                CanCancel = canCancel;
            }
        }
        #endregion

        #region DataGridManipulation

        private void ToggleCellStyle()
        {
            var defaultStyle = (Style)Application.Current.FindResource("DayCellStyle");
            var alternateStyle = (Style)Application.Current.FindResource("AlternateDayCellStyle");

            CurrentCellStyle = CurrentCellStyle == defaultStyle ? alternateStyle : defaultStyle;
        }

        #endregion
        #region TableContentChangeEventHandling
        private void SlotEntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SlotEntry.Name) && sender is SlotEntry slotEntry)
            {
                // Delegate validation logic to SlotEntryValidator
                SlotEntryValidator.ValidateSlotEntry(slotEntry, Students);

                UserInputButtonUpdate(slotEntry);

                // Optional logging
                Console.WriteLine($"SlotEntry.Name changed: {slotEntry.Name}, IsValid: {slotEntry.IsValid}, StudentID: {slotEntry.StudentID}");
            }
        }
        private void UserInputButtonUpdate(SlotEntry slotEntry)
        {
            // Leverage TimeTableSaveStateUpdater for overall state update
            TimeTableSaveStateUpdater.UpdateCanSaveAndCancel(
                _timetableDataBackup,
                TimetableData,
                out var canSave,
                out var canCancel);

            // Incorporate slotEntry.IsValid into CanSave logic
            CanCancel = canCancel;
            CanSave = slotEntry.IsValid && canSave;
            
            // Optional logging
            Console.WriteLine($"Updated CanSave: {CanSave}, CanCancel: {CanCancel}");
        }

        #endregion
        #region Cleanup
        protected override void Cleanup()
        {
            // Unsubscribe from SlotEntry changes to prevent memory leaks
            SubscriptionManager.UnsubscribeFromSlotEntryChanges(_timetableData, SlotEntryPropertyChanged);

            // Unsubscribe from events (from EventAggregator)
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
        #endregion
    }
}
