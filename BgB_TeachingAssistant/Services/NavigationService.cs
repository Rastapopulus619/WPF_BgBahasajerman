using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Models.Interfaces;
using System;

namespace BgB_TeachingAssistant.Services
{
    public class NavigationService
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

        public void NavigateTo(PageDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            var viewModel = GetPageViewModel(descriptor);
            CurrentViewModel = viewModel;
        }

        private IPageViewModel GetPageViewModel(PageDescriptor descriptor)
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
