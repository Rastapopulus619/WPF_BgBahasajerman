using Bgb_DataAccessLibrary.Contracts;
using System;

namespace Bgb_DataAccessLibrary.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceFactory _serviceFactory;
        private IPageViewModel _currentViewModel;

        public event Action CurrentViewModelChanged;

        public NavigationService(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public IPageViewModel CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                if (_currentViewModel != value)
                {
                    DisposeCurrentViewModel();
                    _currentViewModel = value;
                    CurrentViewModelChanged?.Invoke();
                }
            }
        }

        public void NavigateTo(IPageDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            var viewModel = GetPageViewModel(descriptor);

            if(CurrentViewModel is IPageViewModel viewModelType)
            {
                viewModelType.UnsubscribeEvents();
                //viewModelType.EventAggregator.LogSubscriptions();
            }

            CurrentViewModel = viewModel;
        }

        private IPageViewModel GetPageViewModel(IPageDescriptor descriptor)
        {
            var viewModel = _serviceFactory.GetService(descriptor.ViewModelType) as IPageViewModel;
            _serviceFactory.ConfigureServicesFor(viewModel); // Inject services if needed
            return viewModel;
        }

        private void DisposeCurrentViewModel()
        {
            if (_currentViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
