namespace Bgb_DataAccessLibrary.Events
{
    public class PopulateStudentPickerEvent : IPopulateStudentPickerEvent
    {
        public object StudentList { get; }
        public PopulateStudentPickerEvent()
        {
        }
        public PopulateStudentPickerEvent(object studentList)
        {
            StudentList = studentList;
        }
    }
}
