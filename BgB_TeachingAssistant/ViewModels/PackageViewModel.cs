using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Events;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.Commands;
using BgB_TeachingAssistant.Helpers;
using BgB_TeachingAssistant.Services;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;

namespace BgB_TeachingAssistant.ViewModels
{
    public class PackageViewModel : ViewModelBase
    {
        public override string Name => "Packages";
        public IEventAggregator _eventAggregator;
        public IGeneralDataService _generalDataService { get; set; }
        private readonly PackageNavigationService _packageNavigationService;

        // The HtmlContentToNavigateConverter property
        //public HtmlContentToNavigateConverter HtmlContentToNavigateConverter { get; set; }
        private FileSystemWatcher _fileWatcher;

        public IPopulateStudentPickerEvent PopulateStudentPickerEvent { get; set; }
        public PackageCommands PackageCommands { get; }
        public ICommand RefreshContentCommand { get; }

        public ObservableCollection<string> StudentNames { get; private set; }

        private ObservableCollection<StudentPickerStudentModel> _students;

        public ObservableCollection<StudentPickerStudentModel> Students
        {
            get => _students;
            set
            {
                //_students = value;
                //OnPropertyChanged(nameof(Students));
                if (SetProperty(ref _students, value))
                {
                    Console.WriteLine($"First Student is: {_students[0].StudentName}");
                }

            }
        }


        private int _selectedStudentID;
        // This would be bound to the ComboBox
        public int SelectedStudentID
        {
            get { return _selectedStudentID; }
            set
            {
                if (_selectedStudentID != value)
                {
                    _selectedStudentID = value;
                    Console.WriteLine($"Selected StudentID is: {value}");
                    // Do something with the selected StudentID, e.g., database interaction
                }
            }
        }
        private int _selectedPackageNumber;
        public int SelectedPackageNumber
        {
            get => _selectedPackageNumber;
            set => SetProperty(ref _selectedPackageNumber, value);
        }

        private string _htmlDisplayContent;
        public string HtmlDisplayContent
        {
            get => _htmlDisplayContent;
            set
            {
                if (SetProperty(ref _htmlDisplayContent, value))
                {
                    Console.WriteLine($"HtmlDisplayContent changed to: {_htmlDisplayContent}");
                }
            } 
                
        }
        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                if (SetProperty(ref _url, value))
                {
                    Console.WriteLine($"HtmlDisplayContent changed to: {_url}");
                }
            } 
                
        }

        public PackageViewModel(IServiceFactory serviceFactory, IEventAggregator eventAggregator)
            : base(serviceFactory)  // Passing serviceFactory to the base class
        {
            serviceFactory.ConfigureServicesFor(this);

            _eventAggregator = eventAggregator;
            //_packageNavigationService = packageNavigationService;

            //PackageCommands = new PackageCommands(this, packageNavigationService);

            SubscribeToEvent<PopulateStudentPickerEvent>(OnStudentListReceived);

            LoadStudentList();
            SelectedPackageNumber = 1;

            // Initialize the converter in the ViewModel constructor
            //HtmlContentToNavigateConverter = new HtmlContentToNavigateConverter();

            //RefreshContentCommand = new AsyncRelayCommand(async () => await LoadHtmlContentAsync());

            //string localFilePath = @"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\BgB_TeachingAssistant\HtmlContent\TestFiles\testFile.html";
            //Url = localFilePath;

            //InitializeFileWatcher(localFilePath);
            //LoadHtmlContentAsync().ConfigureAwait(false);

            //string htmlContent = "<html><body><h1>Welcome to the Teaching Assistant App</h1><p>This is a sample HTML content.</p></body></html>";
            //HtmlDisplayContent = htmlContent; ///******************** check ChatGPT for instructions how to get the string to XAML!!!
            
        }
        private async void LoadStudentList()
        {
            await _generalDataService.populateStudentPicker();
        }
        private void OnStudentListReceived(PopulateStudentPickerEvent e)
        {
            // Cast the object to the correct type
            if (e.StudentList is List<IStudentPickerStudentModel> studentList)
            {
                // Convert to ObservableCollection
                Students = new ObservableCollection<StudentPickerStudentModel>(studentList.Cast<StudentPickerStudentModel>());
            }
            else
            {
                // Handle the case where the casting fails (optional)
                MessageBox.Show("Failed to cast StudentList.");
            }
        }
        /*
        #region

        private void InitializeFileWatcher(string filePath)
        {
            _fileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(filePath),
                Filter = Path.GetFileName(filePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
            };

            _fileWatcher.Changed += OnHtmlFileChanged;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private async void OnHtmlFileChanged(object sender, FileSystemEventArgs e)
        {
            await LoadHtmlContentAsync();
        }

        private async Task LoadHtmlContentAsync()
        {
            try
            {
                // Read the HTML content from the file asynchronously
                HtmlDisplayContent = await File.ReadAllTextAsync(Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load HTML content: {ex.Message}");
            }
        }

        // Override Cleanup to perform PackageViewModel-specific disposal logic
        protected override void Cleanup()
        {
            // Insert any additional disposal or cleanup logic specific to PackageViewModel here
            Console.WriteLine("PackageViewModel-specific cleanup.");
        }
        
        #endregion
        */

    }

}
