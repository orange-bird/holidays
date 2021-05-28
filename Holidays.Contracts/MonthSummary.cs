namespace Holidays.Contracts
{
    /// <summary>
    /// Month summary containing general stats.
    /// </summary>
    public class MonthSummary
    {
        /// <summary>
        /// Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Codes of countries having holidays this month
        /// </summary>
        public string[] CountryCodes { get; set; }

        /// <summary>
        /// Number of holidays
        /// </summary>
        public int HolidayCount { get; set; }
    }
}
