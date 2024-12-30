using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.ITimeTableDTOs;

public interface ISlotEntry
{
    int StudentID { get; set; }
    string Name { get; set; }
    int SlotID { get; set; }
    string Time { get; set; }
    int DayNumber { get; set; }
    string WeekdayName { get; set; }
    string? Level { get; set; }
    string? Currency { get; set; }
    decimal? Preis { get; set; }
    decimal? DiscountAmount { get; set; }
    string Content { get; set; }
    bool IsEditable { get; set; }
    bool IsValid { get; set; }
    string? Comments { get; set; }
    bool Equals(object obj);
    event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string propertyName = null);
    bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null);
    bool SetPropertyWithLogging<T>(ref T field, T value, [CallerMemberName] string propertyName = null);
}