using System.Globalization;

public static class DateHelper
{
    private static readonly string[] _monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    public static string FormatMonthYear(DateTime date)
    {
        // Format the date and ensure "Sept" is replaced with "Sep"
        string formattedDate = date.ToString("MMM-yy", CultureInfo.InvariantCulture);
        return formattedDate.Replace("Sept", "Sep");
    }

    public static DateTime ParseMonthYear(string monthYear)
    {
        // Replace "Sept" with "Sep" before parsing
        monthYear = monthYear.Replace("Sept", "Sep");
        return DateTime.ParseExact(monthYear, "MMM-yy", CultureInfo.InvariantCulture);
    }

    public static bool TryParseMonthYear(string monthYear, out DateTime date)
    {
        // Replace "Sept" with "Sep" before parsing
        monthYear = monthYear.Replace("Sept", "Sep");
        return DateTime.TryParseExact(monthYear, "MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }
}
