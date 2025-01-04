using System.Collections.ObjectModel;
using System.Linq;

namespace Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs
{
    public class TotalPricesRow : TimeTableRow
    {
        public TotalPricesRow(string zeiten = "Total")
        {
            Zeiten = zeiten;
        }

        // Static method to generate a total prices row
        public static TotalPricesRow CreateFrom(ObservableCollection<TimeTableRow> timeTableRows)
        {
            if (timeTableRows == null || !timeTableRows.Any())
                return new TotalPricesRow(); // Return an empty row if no data

            return new TotalPricesRow
            {
                Zeiten = "Totals:",
                Montag = new SlotEntry
                {
                    Name = "Total",
                    Preis = timeTableRows.Sum(row => row.Montag.Preis ?? 0)
                },
                Dienstag = new SlotEntry
                {
                    Name = "Total",
                    Preis = timeTableRows.Sum(row => row.Dienstag.Preis ?? 0)
                },
                Mittwoch = new SlotEntry
                {
                    Name = "Total",
                    Preis = timeTableRows.Sum(row => row.Mittwoch.Preis ?? 0)
                },
                Donnerstag = new SlotEntry
                {
                    Name = "Total",
                    Preis = timeTableRows.Sum(row => row.Donnerstag.Preis ?? 0)
                },
                Freitag = new SlotEntry
                {
                    Name = "Total",
                    Preis = timeTableRows.Sum(row => row.Freitag.Preis ?? 0)
                },
                Samstag = new SlotEntry
                {
                    Name = "Total",
                    Preis = timeTableRows.Sum(row => row.Samstag.Preis ?? 0)
                },
                Sonntag = new SlotEntry
                {
                    Name = "Total",
                    Preis = timeTableRows.Sum(row => row.Sonntag.Preis ?? 0)
                }
            };
        }
    }
}
