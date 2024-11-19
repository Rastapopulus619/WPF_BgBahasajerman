using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Models.Interfaces;
using Bgb_DataAccessLibrary.Factories;
using BgB_TeachingAssistant.Commands;
using BgB_TeachingAssistant.Services;
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
        private readonly NavigationService _navigationService;

        public ICommand ChangeViewModelCommand { get; }

        public List<PageDescriptor> PageDescriptors { get; }

        public IPageViewModel CurrentPageViewModel => _navigationService.CurrentViewModel;

        public ApplicationViewModel(
            IServiceFactory serviceFactory,
            NavigationService navigationService,
            IEnumerable<PageDescriptor> pageDescriptors,
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
                parameter => _navigationService.NavigateTo((PageDescriptor)parameter),
                parameter => parameter is PageDescriptor);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPageViewModel));
        }
    }

}
