using Bgb_DataAccessLibrary.Models.Interfaces;

namespace Bgb_DataAccessLibrary.Services.Navigation
{
    public interface INavigationService
    {
        IPageViewModel CurrentViewModel { get; }

        event Action CurrentViewModelChanged;

        void NavigateTo(IPageDescriptor descriptor);
    }
}