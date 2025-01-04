using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MVVM_UtilitiesLibrary.BaseClasses;

namespace Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs
{
    public class TotalPricesDisplayModel : ObservableObject
    {
        private string _montagPreis;
        public string MontagPreis
        {
            get => _montagPreis;
            set => SetProperty(ref _montagPreis, value);
        }

        private string _dienstagPreis;
        public string DienstagPreis
        {
            get => _dienstagPreis;
            set => SetProperty(ref _dienstagPreis, value);
        }

        private string _mittwochPreis;
        public string MittwochPreis
        {
            get => _mittwochPreis;
            set => SetProperty(ref _mittwochPreis, value);
        }

        private string _donnerstagPreis;
        public string DonnerstagPreis
        {
            get => _donnerstagPreis;
            set => SetProperty(ref _donnerstagPreis, value);
        }

        private string _freitagPreis;
        public string FreitagPreis
        {
            get => _freitagPreis;
            set => SetProperty(ref _freitagPreis, value);
        }

        private string _samstagPreis;
        public string SamstagPreis
        {
            get => _samstagPreis;
            set => SetProperty(ref _samstagPreis, value);
        }

        private string _sonntagPreis;
        public string SonntagPreis
        {
            get => _sonntagPreis;
            set => SetProperty(ref _sonntagPreis, value);
        }

        /// <summary>
        /// Generates a formatted price display value.
        /// </summary>
        private string GeneratePreisDisplayValue(decimal? preis, decimal? discountAmount)
        {
            if (preis == null)
                return string.Empty;

            decimal calculatedPrice = preis.Value - (discountAmount ?? 0);

            // Create a custom NumberFormatInfo for grouping with dots and commas for decimals
            var customFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            customFormat.NumberGroupSeparator = ".";  // Dot for thousands separator
            customFormat.NumberDecimalSeparator = ","; // Comma for decimals

            // Format the calculated price with grouping and decimals
            string priceString = calculatedPrice.ToString("#,0.##", customFormat);

            return $"IDR {priceString}";
        }

        /// <summary>
        /// Refreshes the display model values based on the provided TimeTableRow collection.
        /// </summary>
        /// <param name="timeTableRows">The collection of TimeTableRow items.</param>
        /// <param name="currency">The currency symbol.</param>
        public void Refresh(ObservableCollection<TimeTableRow> timeTableRows, string currency = "IDR")
        {
            if (timeTableRows == null || !timeTableRows.Any())
            {
                ResetValues();
                return;
            }

            MontagPreis = GeneratePreisDisplayValue(timeTableRows.Sum(row => row.Montag?.IDRPrice ?? 0), timeTableRows.Sum(row => row.Montag?.IDRDiscountAmount ?? 0));
            DienstagPreis = GeneratePreisDisplayValue(timeTableRows.Sum(row => row.Dienstag?.IDRPrice ?? 0), timeTableRows.Sum(row => row.Dienstag?.IDRDiscountAmount ?? 0));
            MittwochPreis = GeneratePreisDisplayValue(timeTableRows.Sum(row => row.Mittwoch?.IDRPrice ?? 0), timeTableRows.Sum(row => row.Mittwoch?.IDRDiscountAmount ?? 0));
            DonnerstagPreis = GeneratePreisDisplayValue(timeTableRows.Sum(row => row.Donnerstag?.IDRPrice ?? 0), timeTableRows.Sum(row => row.Donnerstag?.IDRDiscountAmount ?? 0));
            FreitagPreis = GeneratePreisDisplayValue(timeTableRows.Sum(row => row.Freitag?.IDRPrice ?? 0), timeTableRows.Sum(row => row.Freitag?.IDRDiscountAmount ?? 0));
            SamstagPreis = GeneratePreisDisplayValue(timeTableRows.Sum(row => row.Samstag?.IDRPrice ?? 0), timeTableRows.Sum(row => row.Samstag?.IDRDiscountAmount ?? 0));
            SonntagPreis = GeneratePreisDisplayValue(timeTableRows.Sum(row => row.Sonntag?.IDRPrice ?? 0), timeTableRows.Sum(row => row.Sonntag?.IDRDiscountAmount ?? 0));
        }

        /// <summary>
        /// Resets all values to default.
        /// </summary>
        private void ResetValues()
        {
            MontagPreis = string.Empty;
            DienstagPreis = string.Empty;
            MittwochPreis = string.Empty;
            DonnerstagPreis = string.Empty;
            FreitagPreis = string.Empty;
            SamstagPreis = string.Empty;
            SonntagPreis = string.Empty;
        }
    }
}
