using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.Models.Domain.PackageModels
{
    public class DisplayPackageInfoModel
    {
        public int PackageId { get; set; }
        public int StudentId { get; set; }
        public int? PackageNumber { get; set; }
        public bool PackageCompleted { get; set; }
        public int LessonsAmount { get; set; }
        public int OutstandingLessons { get; set; }
        public int CompletedLessons { get; set; }
    }
}
