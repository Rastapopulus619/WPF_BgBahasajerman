namespace Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventAggregators
{
    public interface IEventAggregator
    {
        //void Subscribe<T>(Action<T> handler);
        void Subscribe<T>(Action<T> handler, IPageViewModel viewModel);
        void Publish<T>(T eventData);
        void Unsubscribe<T>(Action<T> handler);
        void LogSubscriptions();

    }
}
