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

        private Style _mittwochEditingStyle;
        public Style MittwochEditingStyle
        {
            get => _mittwochEditingStyle;
            set => SetProperty(ref _mittwochEditingStyle, value);
        }
        private Style _mittwochTextBlockStyle;
        public Style MittwochTextBlockStyle
        {
            get => _mittwochTextBlockStyle;
            set => SetProperty(ref _mittwochTextBlockStyle, value);
        }


        //continue here: how to use extended xaml techniques here
        private bool _isContentVisible;
        public bool IsContentVisible
        {
            get => _isContentVisible;
            set => SetProperty(ref _isContentVisible, value, nameof(IsContentVisible));
        }
        private bool _arePricesVisible;
        public bool ArePricesVisible
        {
            get => _arePricesVisible;
            set => SetProperty(ref _arePricesVisible, value, nameof(ArePricesVisible));
        }
        private bool _areLevelsVisible;
        public bool AreLevelsVisible
        {
            get => _areLevelsVisible;
            set => SetProperty(ref _areLevelsVisible, value, nameof(AreLevelsVisible));
        }
        private bool _isOverlayVisible = true;
        public bool IsOverlayVisible
        {
            get => _isOverlayVisible;
            set => SetProperty(ref _isOverlayVisible, value, nameof(IsOverlayVisible));
        }
        private string _pricesButtonContent = "Show Prices";
        public string PricesButtonContent
        {
            get => _pricesButtonContent;
            set => SetProperty(ref _pricesButtonContent, value, nameof(PricesButtonContent));
        }
        private Style _currentCellStyle;
        public Style CurrentCellStyle
        {
            get => _currentCellStyle;
            set => SetProperty(ref _currentCellStyle, value);
        }

        public Style DefaultCellStyle { get; set; }
        public Style AlternateCellStyle { get; set; }
        public Style DefaultTextBlockStyle { get; set; }
        public Style ShowLevelCellStyle { get; set; }

        private bool _showLevelsEnabled; // Initialize with a default value
        public bool ShowLevelsEnabled
        {
            get => _showLevelsEnabled;
            set
            {
                if (SetProperty(ref _showLevelsEnabled, value))
                {
                    Console.WriteLine($"ShowLevelsEnabled set to: {value}");
                }
            }
        }

        private TotalPricesDisplayModel _totalPricesDisplayModel = new();
        public TotalPricesDisplayModel TotalPricesDisplayModel
        {
            get => _totalPricesDisplayModel;
            set => SetProperty(ref _totalPricesDisplayModel, value, nameof(TotalPricesDisplayModel));
        }
        private string _testValue = "Initial Value";
        public string TestValue
        {
            get => _testValue;
            set => SetProperty(ref _testValue, value, nameof(TestValue));
        }
        private List<ResourceDictionary> _resourceDictionaries;
        public List<ResourceDictionary> ResourceDictionaries
        {
            get => _resourceDictionaries;
            set
            {
                _resourceDictionaries = value;
                _resourceDictionaries?.ForEach(dictionary =>
                    dictionary.Keys.OfType<object>().ToList().ForEach(key =>
                    {
                        var value = dictionary[key];
                        if (value is Style style)
                        {
                            Console.WriteLine($"Key: {key}");
                            Console.WriteLine($"  TargetType: {style.TargetType}");
                            // Console.WriteLine($"  BasedOn: {(style.BasedOn != null ? style.BasedOn.ToString() : "None")}");
                            // Console.WriteLine($"  Setters: {style.Setters.Count}");
                        }
                        else
                        {
                            Console.WriteLine($"Key: {key}, Value: {value}");
                        }
                    }));
            }
        }

        public Style GetStyleByKey(string key)
        {
            if (ResourceDictionaries == null) return null;

            foreach (var dictionary in ResourceDictionaries)
            {
                if (dictionary.Contains(key))
                {
                    return dictionary[key] as Style;
                }
            }

            Console.WriteLine($"Style with key '{key}' not found.");
            return null;
        }
        public void SetCurrentCellStyle(string styleKey)
        {
            var style = GetStyleByKey(styleKey);
            if (style != null)
            {
                CurrentCellStyle = style;
                Console.WriteLine($"CurrentCellStyle set to style with key: {styleKey}");
            }
            else
            {
                Console.WriteLine($"Failed to set CurrentCellStyle. Style '{styleKey}' not found.");
            }
        }





        #region ViewDataProperties
        private ObservableCollection<IStudentModel> _students;
        public ObservableCollection<IStudentModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value, nameof(Students));
        }
        private ObservableCollection<TimeTableRow> _timeTableDataBackup;
        private ObservableCollection<TimeTableRow> _timeTableData;
        public ObservableCollection<TimeTableRow> TimeTableData
        {
            get => _timeTableData;
            set
            {
                if (SetProperty(ref _timeTableData, value, nameof(TimeTableData)))
                {
                    // UnsubscribeFromSlotEntryChanges(); // Clean up old subscriptions
                    SubscriptionManager.UnsubscribeFromSlotEntryChanges(_timeTableData, SlotEntryPropertyChanged);
                    if (!_isDisposed && _timeTableData != null)
                    {
                        //SubscribeToSlotEntryChanges(); // Subscribe to new data
                        SubscriptionManager.SubscribeToSlotEntryChanges(_timeTableData, SlotEntryPropertyChanged); // Subscribe to new data
                    }

                    FetchTotalPricesRowCollection(); // Update totals row data whenever data changes
                }
            }
        }
        //public double CalculatedRowHeight => TimeTableData.Count > 0 ? DataGridHeight / TimeTableData.Count : 30;

        private ObservableCollection<TotalPricesRow> _totalPricesData;
        public ObservableCollection<TotalPricesRow> TotalPricesData
        {
            get => _totalPricesData;
            set => SetProperty(ref _totalPricesData, value, nameof(TotalPricesData));
        }

        private bool _totalsAreVisible = false;
        public bool TotalsAreVisible
        {
            get => _totalsAreVisible;
            set => SetProperty(ref _totalsAreVisible, value, nameof(TotalsAreVisible));
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
        public ICommand TogglePricesVisibilityCommand { get; }
        public ICommand ToggleCellStyleCommand { get; }
        public ICommand ToggleShowLevels { get; }
        public ICommand ToggleShowTotals { get; }
        public ICommand CloseOverlayCommand { get; }
        #endregion
        #endregion
        public BookedSlotsViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            

            serviceFactory.ConfigureServicesFor(this);

            SaveChangesCommand = new AsyncRelayCommand(ShowSavePrompt);
            RevertChangesCommand = new RelayCommand(ShowRevertPrompt); // no async operations, so use RelayCommand
            ToggleContentVisibilityCommand = new RelayCommand(_ => IsContentVisible = !IsContentVisible);
            TogglePricesVisibilityCommand = new RelayCommand(_ => ExecuteTogglePricesVisibility());
            // Initialize command
            ToggleShowTotals = new RelayCommand(_ => ToggleExpansion());
            CloseOverlayCommand = new RelayCommand(_ => IsOverlayVisible = false);

            DefaultCellStyle = _styles["DayCellStyle"];
            DefaultTextBlockStyle = _styles["ValidationDependentCellStyle"];
            AlternateCellStyle = _styles["AlternateDayCellStyle"];
            CurrentCellStyle = _styles["DayCellStyle"];
            MittwochTextBlockStyle = _styles["ValidationDependentCellStyle"];
            MittwochEditingStyle = _styles["DataGridEditableTextBoxStyle"];
            ShowLevelCellStyle = _styles["LevelDependentCellStyle"];

            ToggleShowLevels = new RelayCommand(_ =>
            {
                // CurrentCellStyle = CurrentCellStyle == DefaultCellStyle
                //     ? ShowLevelCellStyle
                //     : DefaultCellStyle;

                AreLevelsVisible = !AreLevelsVisible;
            });
            // {
            //     ShowLevelsEnabled = !ShowLevelsEnabled;
            // });


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
                TimeTableData = await Initializer.FetchTimeTableDataAsync();
                Students = await Initializer.FetchStudentListAsync();

                TotalPricesData = new ObservableCollection<TotalPricesRow>();

                TotalPricesDisplayModel.Refresh(TimeTableData);

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
            var differences = TimeTableDataHelper.GetDifferences(_timeTableDataBackup, TimeTableData);

            if (BookedSlotsPromptHandler.SavePromptUserChoice(differences, _timeTableDataBackup)) // Delegates to BookedSlotsPromptHandler
            {
                await BookedSlotsDataService.SaveBookedSlotsAsync(differences); // Save operation
                TimeTableData = await Initializer.FetchTimeTableDataAsync(); // Refresh data
                SaveState();
                PromptService.ShowInformationPrompt("Success", $"These Changes:\n\n{BookedSlotsPromptHandler.Changes}\nhave been saved.");
            }
        }
        private void ShowRevertPrompt()
        {
            var differences = TimeTableDataHelper.GetDifferences(_timeTableDataBackup, TimeTableData);

            if (BookedSlotsPromptHandler.RevertPromptUserChoice(differences, _timeTableDataBackup)) // Delegates to BookedSlotsPromptHandler
            {
                RevertToSavedState();
                PromptService.ShowInformationPrompt("Success", $"These Changes:\n\n{BookedSlotsPromptHandler.Changes}\nhave been saved.");
            }
        }
        #endregion
        #region State Logic
        public void SaveState()
        {
            _timeTableDataBackup = TimeTableDataHelper.CloneTimeTableData(TimeTableData);

            // Use intermediate variables for out parameters
            TimeTableSaveStateUpdater.UpdateCanSaveAndCancel(
                _timeTableDataBackup,
                TimeTableData,
                out var canSave,
                out var canCancel);

            // Assign to properties
            CanSave = canSave;
            CanCancel = canCancel;
        }
        public void RevertToSavedState()
        {
            if (_timeTableDataBackup != null)
            {
                TimeTableData = TimeTableDataHelper.CloneTimeTableData(_timeTableDataBackup);

                // Use intermediate variables for out parameters
                TimeTableSaveStateUpdater.UpdateCanSaveAndCancel(
                    _timeTableDataBackup,
                    TimeTableData,
                    out var canSave,
                    out var canCancel);

                // Assign to properties
                CanSave = canSave;
                CanCancel = canCancel;
            }
        }
        #endregion

        public void ToggleExpansion()
        {
            if (!TotalsAreVisible)
            {
                FetchTotalPricesRowCollection();
            }
            TotalsAreVisible = !TotalsAreVisible;
        }
        private void ExecuteTogglePricesVisibility()
        {
            // Existing functionality: toggle the visibility
            ArePricesVisible = !ArePricesVisible;
            if (PricesButtonContent == "Show Prices")
            {
                PricesButtonContent = "Hide Prices";
            }
            else
            {
                PricesButtonContent = "Show Prices";
            }

            // Additional logic can be added here
        }


        #region TableContentChangeEventHandling
        private void SlotEntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SlotEntry.Name) && sender is SlotEntry slotEntry)
            {
                // Delegate validation logic to SlotEntryValidator
                SlotEntryValidator.ValidateSlotEntry(slotEntry, Students);

                UserInputButtonUpdate(slotEntry);

                TotalPricesDisplayModel.Refresh(TimeTableData);

                // Optional logging
                Console.WriteLine($"SlotEntry.Name changed: {slotEntry.Name}, IsValid: {slotEntry.IsValid}, StudentID: {slotEntry.StudentID}");
            }
        }
        private void UserInputButtonUpdate(SlotEntry slotEntry)
        {
            // Leverage TimeTableSaveStateUpdater for overall state update
            TimeTableSaveStateUpdater.UpdateCanSaveAndCancel(
                _timeTableDataBackup,
                TimeTableData,
                out var canSave,
                out var canCancel);

            // Incorporate slotEntry.IsValid into CanSave logic
            CanCancel = canCancel;
            CanSave = slotEntry.IsValid && canSave;
            
            // Optional logging
            Console.WriteLine($"Updated CanSave: {CanSave}, CanCancel: {CanCancel}");
        }
        public void FetchTotalPricesRowCollection()
        {
                

        }


        #endregion
        #region Cleanup
        protected override void Cleanup()
        {
            // Unsubscribe from SlotEntry changes to prevent memory leaks
            SubscriptionManager.UnsubscribeFromSlotEntryChanges(_timeTableData, SlotEntryPropertyChanged);

            // Unsubscribe from events (from EventAggregator)
            UnsubscribeEvents();

            // Clear TimeTableData to break bindings
            if (TimeTableData != null)
            {
                TimeTableData.Clear();
                TimeTableData = null; // Nullify to break binding
            }

            _isDisposed = true; // Set the disposed flag

            // Call base class cleanup
            base.Cleanup();
        }
        #endregion
    }
}
