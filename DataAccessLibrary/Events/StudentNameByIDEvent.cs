namespace Bgb_DataAccessLibrary.Events
{
    public class StudentNameByIDEvent
    {
        public string StudentName { get; }
        public StudentNameByIDEvent(string studentName)
        {
            StudentName = studentName;  
        }
    }
}
