using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.ITimeTableDTOs;
using Bgb_DataAccessLibrary.Contracts.IModels.IStudentModels;

namespace Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs
{
    public class SlotEntryValidator : ISlotEntryValidator
    {
        public void ValidateSlotEntry(ISlotEntry slotEntry, ObservableCollection<IStudentModel> students)
        {
            if (string.IsNullOrWhiteSpace(slotEntry.Name))
            {
                slotEntry.Name = "-";
                slotEntry.IsValid = true;
                slotEntry.StudentID = 0;
                return;
            }

            var matchingStudent = students?.FirstOrDefault(student => student.Name == slotEntry.Name);
            if (matchingStudent != null)
            {
                slotEntry.IsValid = true;
                slotEntry.StudentID = matchingStudent.StudentID;
            }
            else
            {
                slotEntry.IsValid = false;
            }
        }
    }
}