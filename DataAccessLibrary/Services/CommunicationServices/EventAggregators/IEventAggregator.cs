using Bgb_DataAccessLibrary.Models.Interfaces;

namespace Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators
{
    public interface IEventAggregator
    {
        void Subscribe<T>(Action<T> handler);
        void Subscribe<T>(Action<T> handler, IPageViewModel viewModel);
        void Publish<T>(T eventData);
        void Unsubscribe<T>(Action<T> handler);
        void LogSubscriptions();

    }
}
