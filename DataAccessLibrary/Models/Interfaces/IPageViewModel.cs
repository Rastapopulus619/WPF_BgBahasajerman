using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Models.Interfaces;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using System;

namespace Bgb_DataAccessLibrary.Models.Interfaces
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
