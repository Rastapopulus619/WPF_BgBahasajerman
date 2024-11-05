using BgB_TeachingAssistant.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgB_TeachingAssistant.Services
{
    public class DashboardEventTesting
    {
        //*****
        //normal communication
        //Define delegate signature/format
        //public delegate void DataProcessedEventHandler(string data);
        //public event DataProcessedEventHandler DataProcessed;

        public void ProcessData()
        {
            string processedData = "Processed data at " + DateTime.Now;
            
            //*****
            //normal communication
            //DataProcessed?.Invoke(processedData);
        }
        public void ProcessData(EventAggregator eventAggregator)
        {
            string processedData = "Processed data at " + DateTime.Now;
            eventAggregator.Publish(processedData);
        }

    }
}
