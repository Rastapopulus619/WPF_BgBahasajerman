using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Factories;
using BgB_TeachingAssistant.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        public List<IPageViewModel> PageViewModels { get; }

        // The constructor now accepts IEnumerable<IPageViewModel> and assigns it directly.
        public ApplicationViewModel(IServiceFactory serviceFactory, IEnumerable<IPageViewModel> pageViewModels)
            : base(serviceFactory)
        {
            // Populate the PageViewModels list from the injected IEnumerable
            PageViewModels = pageViewModels.ToList();
            CurrentPageViewModel = PageViewModels.FirstOrDefault(); // Set the initial ViewModel
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        ExecuteChangePageCommand,
                        CanExecuteChangePageCommand);
                }
                return _changePageCommand;
            }
        }

        private void ExecuteChangePageCommand(object parameter)
        {
            // Explicitly cast the parameter to IPageViewModel and call the change method.
            if (parameter is IPageViewModel viewModel)
            {
                ChangeViewModel(viewModel);
            }
        }

        private bool CanExecuteChangePageCommand(object parameter)
        {
            // Check if the parameter is of type IPageViewModel.
            return parameter is IPageViewModel;
        }


        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }
        public void ChangeViewModel(IPageViewModel viewModel)
        {
            if (viewModel != null && CurrentPageViewModel != viewModel)
            {
                CurrentPageViewModel = viewModel;

                // Log the name of the current view model to the console
                Console.WriteLine($"Current ViewModel Changed: {CurrentPageViewModel.Name}");
            }
        }
    }
}
