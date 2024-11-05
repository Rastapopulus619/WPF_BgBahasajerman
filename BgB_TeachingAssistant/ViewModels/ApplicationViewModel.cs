using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Factories;
using BgB_TeachingAssistant.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider; // Added for DI
        private ICommand _changePageCommand;
        public ICommand ChangeViewModelCommand => new RelayCommand(ChangeViewModel);

        private IPageViewModel _currentPageViewModel;
        public List<IPageViewModel> PageViewModels { get; }

        // Constructor now accepts IServiceProvider and IEnumerable<IPageViewModel>
        public ApplicationViewModel(IServiceFactory serviceFactory, IServiceProvider serviceProvider, IEnumerable<IPageViewModel> pageViewModels)
            : base(serviceFactory)
        {
            _serviceProvider = serviceProvider; // Assign the injected service provider
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
                    // Dispose of the old ViewModel if it implements IDisposable
                    if (_currentPageViewModel is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }

                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        //public void ChangeViewModel(IPageViewModel viewModel)
        //{
        //    if (viewModel != null && CurrentPageViewModel != viewModel)
        //    {
        //        // Dispose of the current ViewModel if it implements IDisposable
        //        if (CurrentPageViewModel is IDisposable disposable)
        //        {
        //            disposable.Dispose();
        //        }

        //        CurrentPageViewModel = viewModel;

        //        Console.WriteLine($"Switching from {CurrentPageViewModel?.Name} to {viewModel.Name}");
        //    }
        //    else if (CurrentPageViewModel == viewModel)
        //    {
        //        Console.WriteLine($"Attempted to switch to the same ViewModel: {viewModel.Name}");
        //    }
        //}

        //public void ChangeViewModel(IPageViewModel viewModel)
        //{
        //    if (viewModel != null && CurrentPageViewModel != viewModel)
        //    {
        //        // Dispose of the current ViewModel if it implements IDisposable
        //        if (CurrentPageViewModel is IDisposable disposable)
        //        {
        //            disposable.Dispose();
        //        }

        //        // Resolve a new instance of the requested ViewModel from the DI container
        //        _currentPageViewModel = _serviceProvider.GetRequiredService(viewModel.GetType()) as IPageViewModel;

        //        OnPropertyChanged(nameof(CurrentPageViewModel));

        //        Console.WriteLine($"Switching from {CurrentPageViewModel?.Name} to {viewModel.Name}");
        //    }
        //    else if (CurrentPageViewModel == viewModel)
        //    {
        //        Console.WriteLine($"Attempted to switch to the same ViewModel: {viewModel.Name}");
        //    }
        //}

        /*
        public void ChangeViewModel(IPageViewModel viewModel)
        {
            if (viewModel != null && CurrentPageViewModel?.GetType() != viewModel.GetType())
            {
                // Dispose of the current ViewModel if it implements IDisposable
                if (CurrentPageViewModel is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                // Resolve a new instance of the ViewModel from the DI container
                CurrentPageViewModel = _serviceProvider.GetRequiredService(viewModel.GetType()) as IPageViewModel;

                Console.WriteLine($"Switching from {CurrentPageViewModel?.Name} to {viewModel.Name}");
            }
            else if (CurrentPageViewModel == viewModel)
            {
                Console.WriteLine($"Attempted to switch to the same ViewModel: {viewModel.Name}");
            }
        }
        */

        private void ChangeViewModel(object parameter)
        {
            if (parameter is IPageViewModel viewModel)
            {
                // Dispose of the current ViewModel if it implements IDisposable
                if (CurrentPageViewModel is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                // Resolve a new instance of the ViewModel from the DI container
                CurrentPageViewModel = _serviceProvider.GetRequiredService(viewModel.GetType()) as IPageViewModel;

                Console.WriteLine($"Switched to {CurrentPageViewModel?.Name}");
            }
        }


        public void NavigateTo<TViewModel>() where TViewModel : IPageViewModel
        {
            // Resolve the new ViewModel from the DI container
            CurrentPageViewModel = _serviceProvider.GetRequiredService<TViewModel>();
        }
    }
}
