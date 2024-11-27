namespace Bgb_DataAccessLibrary.Contracts
{
    public interface INavigationService
    {
        IPageViewModel CurrentViewModel { get; }

        event Action CurrentViewModelChanged;

        void NavigateTo(IPageDescriptor descriptor);
    }
}