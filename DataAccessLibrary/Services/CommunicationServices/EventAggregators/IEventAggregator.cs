using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators
{
    public interface IEventAggregator
    {
        void Subscribe<T>(Action<T> handler);
        void Publish<T>(T eventData);
    }
}
