using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Models.Interfaces;
using Bgb_DataAccessLibrary.Factories;
using BgB_TeachingAssistant.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public IServiceProvider _serviceProvider { get; set; } // Added for DI
        private readonly IEventAggregator _eventAggregator;

        private ICommand _changePageCommand;
        public ICommand ChangeViewModelCommand => new RelayCommand(obj => ChangeViewModel((PageDescriptor)obj));

        private IPageViewModel _currentPageViewModel;
        public List<IPageViewModel> PageViewModels { get; }
        public List<PageDescriptor> PageDescriptors { get; }

        // Constructor now accepts IServiceProvider and IEnumerable<IPageViewModel>
        public ApplicationViewModel(IServiceFactory serviceFactory, IServiceProvider serviceProvider, IEnumerable<PageDescriptor> pageDescriptors, IEventAggregator eventAggregator)
            : base(serviceFactory, eventAggregator)
        {

            serviceFactory.ConfigureServicesFor(this);


            //if (serviceProvider is LoggingServiceProvider)
            //{
            //    Console.WriteLine("[DI] LoggingServiceProvider is active.");
            //}
            //else
            //{
            //    Console.WriteLine("[DI] Default IServiceProvider detected (logging may not work).");
            //}

            //_serviceProvider = serviceProvider; // Assign the injected service provider
            _eventAggregator = eventAggregator;

            PageDescriptors = pageDescriptors.ToList();
            CurrentPageViewModel = GetPageViewModel(PageDescriptors.FirstOrDefault()); // Set the initial ViewModel

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
            if (parameter is PageDescriptor descriptor)
            {
                ChangeViewModel(descriptor);
            }
        }

        private bool CanExecuteChangePageCommand(object parameter)
        {
            // Check if the parameter is of type IPageViewModel.
            return parameter is PageDescriptor;
        }

        public IPageViewModel CurrentPageViewModel
        {
            get => _currentPageViewModel ??= GetPageViewModel(PageDescriptors.FirstOrDefault());
            set
            {
                if (_currentPageViewModel != value)
                {
                    DisposeCurrentViewModel();
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        private void ChangeViewModel(PageDescriptor descriptor)
        {
            if (descriptor != null)
            {
                CurrentPageViewModel = GetPageViewModel(descriptor);
            }
        }
        // Dispose the current ViewModel if it's IDisposable
        private void DisposeCurrentViewModel()
        {
            if (_currentPageViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        private IPageViewModel GetPageViewModel(PageDescriptor descriptor)
        {
            return _serviceProvider.GetRequiredService(descriptor.ViewModelType) as IPageViewModel;
        }

        public void NavigateTo<TViewModel>() where TViewModel : IPageViewModel
        {
            // Resolve the new ViewModel from the DI container
            CurrentPageViewModel = _serviceProvider.GetRequiredService<TViewModel>();
        }
    }
}
