using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary;
using BgB_TeachingAssistant.Services;
using System.Data;
using System.Security.AccessControl;
using Bgb_DataAccessLibrary.Events;
using System.Formats.Tar;

namespace BgB_TeachingAssistant.ViewModels
{
    class StudentManagerViewModel : ViewModelBase
    {
        public override string Name => "StudentManager";

        private ObservableCollection<TimeTableRow> _timetableData;
        public ObservableCollection<TimeTableRow> TimetableData
        {
                get => _timetableData;
                set => SetProperty(ref _timetableData, value, nameof(TimetableData));
        }


        public StudentManagerViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            serviceFactory.ConfigureServicesFor(this);

            TimetableData = new ObservableCollection<TimeTableRow>
{
    new TimeTableRow
    {
        Zeiten = "07:00-08:30",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "Brenda", IsEditable = false },
        Donnerstag = new DayEntry { Content = "-" },
        Freitag = new DayEntry { Content = "Gemiano" },
        Samstag = new DayEntry { Content = "Alexis" },
        Sonntag = new DayEntry { Content = "Keaness" }
    },
    new TimeTableRow
    {
        Zeiten = "08:30-10:00",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "Lulu" },
        Donnerstag = new DayEntry { Content = "Henny" },
        Freitag = new DayEntry { Content = "Lulu" },
        Samstag = new DayEntry { Content = "Raynanda" },
        Sonntag = new DayEntry { Content = "Yuni" }
    },
    new TimeTableRow
    {
        Zeiten = "10:00-11:30",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "Theodore" },
        Donnerstag = new DayEntry { Content = "Fathma" },
        Freitag = new DayEntry { Content = "Lulu" },
        Samstag = new DayEntry { Content = "Louisa" },
        Sonntag = new DayEntry { Content = "Terrence" }
    },
    new TimeTableRow
    {
        Zeiten = "13:00-14:30",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "Alexander" },
        Donnerstag = new DayEntry { Content = "Alexander" },
        Freitag = new DayEntry { Content = "Alexander" },
        Samstag = new DayEntry { Content = "Anton" },
        Sonntag = new DayEntry { Content = "Febi" }
    },
    new TimeTableRow
    {
        Zeiten = "14:30-16:00",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "Anton" },
        Donnerstag = new DayEntry { Content = "Lulu" },
        Freitag = new DayEntry { Content = "Febi" },
        Samstag = new DayEntry { Content = "Meysha" },
        Sonntag = new DayEntry { Content = "Albert" }
    },
    new TimeTableRow
    {
        Zeiten = "16:00-17:30",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "Sharon" },
        Donnerstag = new DayEntry { Content = "Meysha" },
        Freitag = new DayEntry { Content = "Kenneth" },
        Samstag = new DayEntry { Content = "Try" },
        Sonntag = new DayEntry { Content = "Dzulfiqar" }
    },
    new TimeTableRow
    {
        Zeiten = "17:30-19:00",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "Dzulfiqar" },
        Donnerstag = new DayEntry { Content = "Kenneth R." },
        Freitag = new DayEntry { Content = "Michael" },
        Samstag = new DayEntry { Content = "Vioness" },
        Sonntag = new DayEntry { Content = "Kenneth" }
    },
    new TimeTableRow
    {
        Zeiten = "19:00-20:30",
        Montag = new DayEntry { Content = "-" },
        Dienstag = new DayEntry { Content = "-" },
        Mittwoch = new DayEntry { Content = "-" },
        Donnerstag = new DayEntry { Content = "-" },
        Freitag = new DayEntry { Content = "-" },
        Samstag = new DayEntry { Content = "-" },
        Sonntag = new DayEntry { Content = "-" }
    }


};



            TimetableData[0].Montag.Content = "Updated Name";
            TimetableData[0].Montag.IsValid = false; // Mark as invalid
        }
        public class TimeTableRow
        {
            public string Zeiten { get; set; }     // Maps to the "Time" column
                                                   // Day entries for each day of the week
            public DayEntry Montag { get; set; } = new DayEntry();
            public DayEntry Dienstag { get; set; } = new DayEntry();
            public DayEntry Mittwoch { get; set; } = new DayEntry();
            public DayEntry Donnerstag { get; set; } = new DayEntry();
            public DayEntry Freitag { get; set; } = new DayEntry();
            public DayEntry Samstag { get; set; } = new DayEntry();
            public DayEntry Sonntag { get; set; } = new DayEntry();
        }
        // Represents additional metadata for a day's entry
        public class DayEntry : ObservableObject
        {
            private string _content;
            private bool _isEditable = true;
            private bool _isValid = true;
            private string _comments;

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

            public string Comments
            {
                get => _comments;
                set => SetProperty(ref _comments, value);
            }
        }
        // Initialize TimetableData
        protected override void Cleanup()
        {
            // Unsubscribe from events
            UnsubscribeEvents();

            // Clear TimetableData to break bindings
            if (TimetableData != null)
            {
                TimetableData.Clear();
                TimetableData = null; // Nullify to break binding
            }

            // Call base class cleanup
            base.Cleanup();
        }
    }
}
