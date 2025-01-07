using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Contracts.IModels.IDomain.IStudentModels;

namespace Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.ITimeTableDTOs
{
    public interface ISlotEntryValidator
    {
        void ValidateSlotEntry(ISlotEntry slotEntry, ObservableCollection<IStudentModel> students);
    }
}
