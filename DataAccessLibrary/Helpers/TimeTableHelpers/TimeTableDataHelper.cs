using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;
using Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers;

namespace Bgb_DataAccessLibrary.Helpers.TimeTableHelpers
{
    public class TimeTableDataHelper : ITimeTableDataHelper
    {
        public ObservableCollection<TimeTableRow> CloneTimetableData(ObservableCollection<TimeTableRow> original)
        {
            if (original == null) return null;

            // LINQ-based implementation
            return new ObservableCollection<TimeTableRow>(
                original.Select(row => new TimeTableRow
                {
                    Zeiten = row.Zeiten,
                    Montag = CloneSlotEntry(row.Montag),
                    Dienstag = CloneSlotEntry(row.Dienstag),
                    Mittwoch = CloneSlotEntry(row.Mittwoch),
                    Donnerstag = CloneSlotEntry(row.Donnerstag),
                    Freitag = CloneSlotEntry(row.Freitag),
                    Samstag = CloneSlotEntry(row.Samstag),
                    Sonntag = CloneSlotEntry(row.Sonntag)
                }));

            // Non-LINQ version (commented out for future use)
            /*
            var clone = new ObservableCollection<TimeTableRow>();
            foreach (var row in original)
            {
                var clonedRow = new TimeTableRow
                {
                    Zeiten = row.Zeiten,
                    Montag = CloneSlotEntry(row.Montag),
                    Dienstag = CloneSlotEntry(row.Dienstag),
                    Mittwoch = CloneSlotEntry(row.Mittwoch),
                    Donnerstag = CloneSlotEntry(row.Donnerstag),
                    Freitag = CloneSlotEntry(row.Freitag),
                    Samstag = CloneSlotEntry(row.Samstag),
                    Sonntag = CloneSlotEntry(row.Sonntag),
                };

                clone.Add(clonedRow);
            }

            return clone;
            */
        }

        private SlotEntry CloneSlotEntry(SlotEntry original)
        {
            if (original == null) return null;
            return new SlotEntry
            {
                StudentID = original.StudentID,
                Name = original.Name,
                SlotID = original.SlotID,
                Time = original.Time,
                DayNumber = original.DayNumber,
                WeekdayName = original.WeekdayName,
                Level = original.Level,
                Currency = original.Currency,
                Preis = original.Preis,
                DiscountAmount = original.DiscountAmount,
                Content = original.Content,
                IsEditable = original.IsEditable,
                IsValid = original.IsValid,
                Comments = original.Comments
            };
        }

        public bool AreTimetableDataEqual(ObservableCollection<TimeTableRow> original, ObservableCollection<TimeTableRow> updated)
        {
            // Null checks for both collections + Check if counts are different
            if (original == null || updated == null || original.Count != updated.Count)
                return false;

            for (int i = 0; i < original.Count; i++)
            {
                if (!AreTimeTableRowsEqual(original[i], updated[i]))
                    return false;
            }

            return true;
        }

        private bool AreTimeTableRowsEqual(TimeTableRow row1, TimeTableRow row2)
        {
            if (row1 == null || row2 == null) return false;

            return AreSlotEntriesEqual(row1.Montag, row2.Montag) &&
                   AreSlotEntriesEqual(row1.Dienstag, row2.Dienstag) &&
                   AreSlotEntriesEqual(row1.Mittwoch, row2.Mittwoch) &&
                   AreSlotEntriesEqual(row1.Donnerstag, row2.Donnerstag) &&
                   AreSlotEntriesEqual(row1.Freitag, row2.Freitag) &&
                   AreSlotEntriesEqual(row1.Samstag, row2.Samstag) &&
                   AreSlotEntriesEqual(row1.Sonntag, row2.Sonntag);
        }

        private bool AreSlotEntriesEqual(SlotEntry entry1, SlotEntry entry2)
        {
            if (entry1 == null || entry2 == null) return false;

            return entry1.StudentID == entry2.StudentID &&
                   entry1.Name == entry2.Name &&
                   entry1.SlotID == entry2.SlotID &&
                   entry1.Time == entry2.Time &&
                   entry1.DayNumber == entry2.DayNumber &&
                   entry1.WeekdayName == entry2.WeekdayName &&
                   entry1.Level == entry2.Level &&
                   entry1.Currency == entry2.Currency &&
                   entry1.Preis == entry2.Preis &&
                   entry1.DiscountAmount == entry2.DiscountAmount &&
                   entry1.Content == entry2.Content &&
                   entry1.IsEditable == entry2.IsEditable &&
                   entry1.IsValid == entry2.IsValid &&
                   entry1.Comments == entry2.Comments;
        }

        public List<SlotEntry> GetDifferences(ObservableCollection<TimeTableRow> original, ObservableCollection<TimeTableRow> updated)
        {
            var differences = new List<SlotEntry>();

            if (original == null || updated == null)
                throw new InvalidOperationException("Timetable data is null.");

            for (int i = 0; i < original.Count; i++)
            {
                var originalRow = original[i];
                var updatedRow = updated[i];

                CompareSlotEntries(differences, originalRow.Montag, updatedRow.Montag);
                CompareSlotEntries(differences, originalRow.Dienstag, updatedRow.Dienstag);
                CompareSlotEntries(differences, originalRow.Mittwoch, updatedRow.Mittwoch);
                CompareSlotEntries(differences, originalRow.Donnerstag, updatedRow.Donnerstag);
                CompareSlotEntries(differences, originalRow.Freitag, updatedRow.Freitag);
                CompareSlotEntries(differences, originalRow.Samstag, updatedRow.Samstag);
                CompareSlotEntries(differences, originalRow.Sonntag, updatedRow.Sonntag);
            }

            return differences;
        }

        private void CompareSlotEntries(List<SlotEntry> differences, SlotEntry original, SlotEntry updated)
        {
            // Check null + Compare properties and add to differences if any mismatch is found
            if (original == null || updated == null || !original.Equals(updated))
                differences.Add(updated);
        }
    }
}
