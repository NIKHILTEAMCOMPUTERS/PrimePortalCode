using System.Globalization;

namespace RMS.Client.Models.Timesheet
{
    public class MonthlyEntryCount
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int EntryCount { get; set; }
        public string MonthYear => $"{GetMonthName(Month).ToLower()}-{Year.ToString().Substring(2)}"; // e.g., "jun-24"

        private string GetMonthName(int month)
        {
            return new DateTime(1, month, 1).ToString("MMM", CultureInfo.InvariantCulture);
        }
    }
}
