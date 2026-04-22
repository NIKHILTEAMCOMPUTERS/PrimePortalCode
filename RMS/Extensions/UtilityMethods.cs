using System.Text.RegularExpressions;

namespace RMS.Extensions
{
    public static class UtilityMethods
    {
        public  static bool IsValidMonthYearFormat(string monthyear)
        {
            //  regular expression pattern for the monthyear format
            string pattern = @"^\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-\d{2}\b$";          
            Regex regex = new Regex(pattern);           
            return regex.IsMatch(monthyear);
        }
    }
}
