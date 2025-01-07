namespace Bgb_DataAccessLibrary.Contracts.IModels.IDisplayModels
{
    public interface IStudentManagerDisplayStudentModel
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
        public string CityOfResidence { get; set; }
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
    }
}