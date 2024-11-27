using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Data.DataServices;
using System;

namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IPageViewModel
    {
        string Name { get; }
        int EventHandlersCount { get; }
        IEventAggregator EventAggregator { get; }
        public List<Delegate> EventHandlers { get; set; }
        void UnsubscribeEvents();
    }
}
