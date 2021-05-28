namespace Holidays.Contracts
{
    /// <summary>
    /// Country holidays summary.
    /// </summary>
    public class CountrySummary
    {
        /// <summary>
        /// Country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Number of holidays
        /// </summary>
        public int HolidayCount { get; set; }
    }
}
