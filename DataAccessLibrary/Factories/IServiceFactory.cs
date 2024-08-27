using Bgb_DataAccessLibrary.Data.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.Factories
{
    public interface IServiceFactory
    {
        GeneralDataService CreateGeneralDataService();
        StudentProfileDataService CreateStudentProfileDataService();

        // Other services...
    }
}
