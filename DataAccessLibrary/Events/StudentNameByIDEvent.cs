namespace Bgb_DataAccessLibrary.Events
{
    public class StudentNameByIDEvent : IStudentNameByIDEvent
    {
        public string StudentName { get; }
        public StudentNameByIDEvent()
        {
        }
        public StudentNameByIDEvent(string studentName)
        {
            StudentName = studentName;
        }
    }
}
