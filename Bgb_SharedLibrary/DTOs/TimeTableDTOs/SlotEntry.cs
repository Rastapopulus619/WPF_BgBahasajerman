using MVVM_UtilitiesLibrary.BaseClasses;

namespace Bgb_SharedLibrary.DTOs.TimeTableDTOs
{
    public class SlotEntry : ObservableObject
    {
        private int _studentID;
        private string _name;
        private int _slotID;
        private int _dayNumber;
        private string _weekdayName;
        private string? _level; // Nullable
        private string? _currency; // Nullable
        private decimal? _preis; // Nullable
        private decimal? _discountAmount; // Nullable

        private string _content;
        private bool _isEditable = true;
        private bool _isValid = true;
        private string? _comments;

        public int StudentID
        {
            get => _studentID;
            set => SetProperty(ref _studentID, value);
        }

        public string Name
        {
            get => _name;
            set => SetPropertyWithLogging(ref _name, value);
            //set => SetProperty(ref _name, value);
        }

        public int SlotID
        {
            get => _slotID;
            set => SetProperty(ref _slotID, value);
        }

        public int DayNumber
        {
            get => _dayNumber;
            set => SetProperty(ref _dayNumber, value);
        }

        public string WeekdayName
        {
            get => _weekdayName;
            set => SetProperty(ref _weekdayName, value);
        }

        public string? Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        public string? Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        public decimal? Preis
        {
            get => _preis;
            set => SetProperty(ref _preis, value);
        }

        public decimal? DiscountAmount
        {
            get => _discountAmount;
            set => SetProperty(ref _discountAmount, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public bool IsEditable
        {
            get => _isEditable;
            set => SetProperty(ref _isEditable, value);
        }

        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        public string? Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }
        public override bool Equals(object obj)
        {
            if (obj is not SlotEntry other) return false;

            return StudentID == other.StudentID &&
                   Name == other.Name &&
                   SlotID == other.SlotID &&
                   DayNumber == other.DayNumber &&
                   WeekdayName == other.WeekdayName &&
                   Level == other.Level &&
                   Currency == other.Currency &&
                   Preis == other.Preis &&
                   DiscountAmount == other.DiscountAmount &&
                   Content == other.Content &&
                   IsEditable == other.IsEditable &&
                   IsValid == other.IsValid &&
                   Comments == other.Comments;
        }

    }
}
