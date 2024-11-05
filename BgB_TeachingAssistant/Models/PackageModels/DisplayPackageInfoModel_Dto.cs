using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgB_TeachingAssistant.Models.PackageModels
{
    public class DisplayPackageInfoModel_Dto
    {
        public int PackageId { get; set; }
        public int StudentId { get; set; }
        public int? PackageNumber { get; set; }
        public bool IsCompleted { get; set; }
        public int TotalLessons { get; set; }
        public int RemainingLessons { get; set; }
        public int FinishedLessons { get; set; }
    }
}
