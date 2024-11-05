using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.Commands;
using BgB_TeachingAssistant.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class PackageViewModel : ViewModelBase
    {
        public override string Name => "Packages";
        private readonly GeneralDataService _generalDataService;
        private readonly PackageNavigationService _packageNavigationService;
        public PackageCommands PackageCommands { get; }

        public ObservableCollection<string> StudentNames { get; private set; }

        private int _selectedPackageNumber = 3;
        public int SelectedPackageNumber
        {
            get => _selectedPackageNumber;
            set => SetProperty(ref _selectedPackageNumber, value);
        }

        public PackageViewModel(IServiceFactory serviceFactory, GeneralDataService generalDataService, PackageNavigationService packageNavigationService)
            : base(serviceFactory)  // Passing serviceFactory to the base class
        {
            _generalDataService = generalDataService;
            _packageNavigationService = packageNavigationService;

            PackageCommands = new PackageCommands(this, packageNavigationService);
            LoadStudentList();
        }

        private async void LoadStudentList()
        {
            try
            {
                var studentNames = await _generalDataService.GetStudentNamesAsync();
                StudentNames = new ObservableCollection<string>(studentNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving student list: {ex.Message}");
            }
        }
    }

}
