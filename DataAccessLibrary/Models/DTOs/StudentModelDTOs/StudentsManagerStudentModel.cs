/*using System.Data;
using Bgb_DataAccessLibrary.Contracts.IModels.IDisplayModels;
using Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.IStudentModelDTOs;

    namespace Bgb_DataAccessLibrary.Models.DTOs.StudentModelDTOs
    {
        public class StudentsManagerStudentModel : IStudentsManagerStudentModel
        {
            private readonly IStudentManagerDisplayStudentModel _displayModel;

            public StudentsManagerStudentModel(IStudentManagerDisplayStudentModel displayModel)
            {
                _displayModel = displayModel ?? throw new ArgumentNullException(nameof(displayModel));
            }

            public void PopulateFromDataRow(DataRow row)
            {
                if (row == null)
                    throw new ArgumentNullException(nameof(row));

                _displayModel.StudentID = row.Field<int>("StudentID");
                _displayModel.StudentNumber = row.Field<int>("StudentNumber");
                _displayModel.Name = row.Field<string>("Name") ?? "N/A";
                _displayModel.Title = row.Field<string>("Title") ?? "N/A";
                _displayModel.Goal = row.Field<string>("Goal") ?? "N/A";
                _displayModel.Level = row.Field<string>("Level") ?? "N/A";
                _displayModel.FirstLesson = row.Field<DateTime?>("FirstLesson") ?? DateTime.MinValue;
                _displayModel.LessonsTotal = row.Field<int?>("LessonsTotal") ?? 0;
                _displayModel.LastLesson = row.Field<DateTime?>("LastLesson") ?? DateTime.MinValue;
                _displayModel.StudyStatus = row.Field<string>("StudyStatus") ?? "Unknown";
                _displayModel.CityOfResidence = row.Field<string>("CityOfResidence") ?? "N/A";
                _displayModel.CountryOfResidence = row.Field<string>("CountryOfResidence") ?? "N/A";
                _displayModel.ProvinceOfResidence = row.Field<string>("ProvinceOfResidence") ?? "N/A";
                _displayModel.AddressField = row.Field<string>("AddressField") ?? "N/A";
                _displayModel.Email1 = row.Field<string>("Email1") ?? "N/A";
                _displayModel.Email2 = row.Field<string>("Email2") ?? "N/A";
                _displayModel.Tel1 = row.Field<string>("Tel1") ?? "N/A";
                _displayModel.Tel2 = row.Field<string>("Tel2") ?? "N/A";
                _displayModel.TelOrtu1 = row.Field<string>("TelOrtu1") ?? "N/A";
                _displayModel.TelOrtu2 = row.Field<string>("TelOrtu2") ?? "N/A";
                _displayModel.Age = row.Field<int?>("Age") ?? 0;
                _displayModel.Gender = row.Field<string>("Gender") ?? "Unknown";
                _displayModel.CountryOfOrigin = row.Field<string>("CountryOfOrigin") ?? "N/A";
                _displayModel.ProvinceOfOrigin = row.Field<string>("ProvinceOfOrigin") ?? "N/A";
                _displayModel.CityOfOrigin = row.Field<string>("CityOfOrigin") ?? "N/A";
            }

            public IStudentManagerDisplayStudentModel GetDisplayModel() => _displayModel;
        }
    }

}*/
