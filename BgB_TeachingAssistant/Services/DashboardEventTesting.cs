using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using Bgb_DataAccessLibrary.Data.DataServices;

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
        public async Task ProcessData(IEventAggregator eventAggregator, GeneralDataService dataService)
        {
            List<string> studentNames = await dataService.GetStudentNamesAsync();
            string processedData = "Processed data at " + DateTime.Now;
            eventAggregator.Publish(processedData);
        }

    }
}
