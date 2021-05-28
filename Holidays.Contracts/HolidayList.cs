namespace Holidays.Contracts
{
    /// <summary>
    /// Holiday list.
    /// </summary>
    public class HolidayList
    {
        /// <summary>
        /// Country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Holidays
        /// </summary>
        public Holiday[] Holidays { get; set; }
    }
}
