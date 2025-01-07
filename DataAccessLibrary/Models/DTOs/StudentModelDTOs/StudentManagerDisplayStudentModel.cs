using Bgb_DataAccessLibrary.Contracts.IModels.IDisplayModels;
using MVVM_UtilitiesLibrary.BaseClasses;
using System.ComponentModel;
using Bgb_SharedLibrary.SharedCollections;

namespace Bgb_DataAccessLibrary.Models.DTOs.StudentModelDTOs
{
    public class StudentManagerDisplayStudentModel : ObservableObject, IStudentManagerDisplayStudentModel, IDataErrorInfo
    {

        public int StudentID { get; set; }
        public int StudentNumber { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public string Level { get; set; }
        public DateTime FirstLesson { get; set; }
        public int LessonsTotal { get; set; }
        public DateTime LastLesson { get; set; }
        public string StudyStatus { get; set; }

        private string _cityOfResidence;
        public string CityOfResidence
        {
            get => _cityOfResidence;
            set => SetProperty(ref _cityOfResidence, value);
        }

        public string CountryOfResidence { get; set; }
        public string ProvinceOfResidence { get; set; }
        public string AddressField { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string TelOrtu1 { get; set; }
        public string TelOrtu2 { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string CountryOfOrigin { get; set; }
        public string ProvinceOfOrigin { get; set; }
        public string CityOfOrigin { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case nameof(CityOfResidence):
                        error = ValidateCityOfResidence();
                        break;
                    case nameof(Title):
                        error = ValidateTitle();
                        break;

                        // Add validation cases for other properties
                }

                return error;
            }
        }

        private string ValidateCityOfResidence()
        {
            if (string.IsNullOrEmpty(CityOfResidence))
            {
                return "City of Residence cannot be empty.";
            }

            if (CityOfResidence.Length > 50)
            {
                return "City of Residence cannot exceed 50 characters.";
            }

            return null;
        }
        private string ValidateTitle()
        {
            if (string.IsNullOrWhiteSpace(Title) || AllowedTitles.Titles.Contains(Title))
            {
                return null; // Valid
            }

            return $"Title must be: {string.Join(", ", AllowedTitles.Titles)}, or empty.";
        }

    }
}
