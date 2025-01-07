using Bgb_DataAccessLibrary.Contracts.IModels.IDisplayModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.IStudentModelDTOs
{
    public interface IStudentsManagerStudentModel
    {
        int StudentID { get; set; }
        int StudentNumber { get; set; }
        string Name { get; set; }
        string Title { get; set; }
        string Goal { get; set; }
        string Level { get; set; }
        DateTime? FirstLesson { get; set; }
        int? LessonsTotal { get; set; }
        DateTime? LastLesson { get; set; }
        string StudyStatus { get; set; }
        string CityOfResidence { get; set; }
        string CountryOfResidence { get; set; }
        string ProvinceOfResidence { get; set; }
        string AddressField { get; set; }
        string Email1 { get; set; }
        string Email2 { get; set; }
        string Tel1 { get; set; }
        string Tel2 { get; set; }
        string TelOrtu1 { get; set; }
        string TelOrtu2 { get; set; }
        int? Age { get; set; }
        string Gender { get; set; }
        string CountryOfOrigin { get; set; }
        string ProvinceOfOrigin { get; set; }
        string CityOfOrigin { get; set; }

        IStudentManagerDisplayStudentModel ToDisplayModel();
    }
}
