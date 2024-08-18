using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentNumber { get; set; }
        public string Name { get; set; }
        public string Title { get; set; } // Nullable, so use string (if nullable) or set a default
    }
}
