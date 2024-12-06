using Bgb_DataAccessLibrary.Databases;
using BgB_TeachingAssistant.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Bgb_DataAccessLibrary.Contracts;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        private IPageViewModel _currentPageViewModel;
        public INavigationService _navigationService { get; set; }
        public ICommand ChangeViewModelCommand { get; private set; }

        public IEnumerable<IPageDescriptor> PageDescriptors { get; set; }

        public IPageViewModel CurrentPageViewModel
        {
            get => _navigationService.CurrentViewModel;
            private set => SetProperty(ref _currentPageViewModel, value);
        }

        public ApplicationViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            // Configure necessary services for this view model
            serviceFactory.ConfigureServicesFor(this);

            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            if (PageDescriptors == null || !PageDescriptors.Any())
                throw new InvalidOperationException("No page descriptors provided.");

            _navigationService.NavigateTo(PageDescriptors.First());

            ChangeViewModelCommand = new RelayCommand(
                parameter => _navigationService.NavigateTo((IPageDescriptor)parameter),
                parameter => parameter is PageDescriptor);
        }

        private void OnCurrentViewModelChanged()
        {
            //OnPropertyChanged(nameof(CurrentPageViewModel));
            CurrentPageViewModel = _navigationService.CurrentViewModel;
        }
        protected override void Cleanup()
        {
            _navigationService.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Cleanup();
        }
    }
}
