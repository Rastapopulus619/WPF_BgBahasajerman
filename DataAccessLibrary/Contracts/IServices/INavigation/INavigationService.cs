namespace Bgb_DataAccessLibrary.Contracts.IServices.INavigation
{
    public interface INavigationService
    {
        IPageViewModel CurrentViewModel { get; }

        event Action CurrentViewModelChanged;

        void NavigateTo(IPageDescriptor descriptor);
    }
}