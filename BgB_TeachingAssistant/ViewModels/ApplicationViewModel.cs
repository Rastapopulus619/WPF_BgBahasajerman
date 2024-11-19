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
using Bgb_DataAccessLibrary.Services.Navigation;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public INavigationService _navigationService;

        public ICommand ChangeViewModelCommand { get; }

        public List<IPageDescriptor> PageDescriptors { get; }

        public IPageViewModel CurrentPageViewModel => _navigationService.CurrentViewModel;

        public ApplicationViewModel(
            IServiceFactory serviceFactory,
            INavigationService navigationService,
            IEnumerable<IPageDescriptor> pageDescriptors,
            IEventAggregator eventAggregator)
            : base(serviceFactory, eventAggregator)
        {
            _navigationService = navigationService;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            PageDescriptors = pageDescriptors.ToList();

            if (!PageDescriptors.Any())
                throw new InvalidOperationException("No page descriptors provided.");

            _navigationService.NavigateTo(PageDescriptors.First());

            ChangeViewModelCommand = new RelayCommand(
                parameter => _navigationService.NavigateTo((IPageDescriptor)parameter),
                parameter => parameter is PageDescriptor);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPageViewModel));
        }
    }

}
