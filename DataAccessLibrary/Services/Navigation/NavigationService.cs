using Bgb_DataAccessLibrary.Contracts;
using Bgb_DataAccessLibrary.Contracts.IServices.INavigation;
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

            // Dispose and force GC before navigating to new ViewModel
            DisposeCurrentViewModel();

            // Force garbage collection to test destructor
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var viewModel = GetPageViewModel(descriptor);

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
            _currentViewModel = null;
        }
    }
}
