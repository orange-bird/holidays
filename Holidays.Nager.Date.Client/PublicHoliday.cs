using System;

namespace Holidays.Nager.Date
{
    /// <summary>
    /// Public Holiday.
    /// Copied from https://raw.githubusercontent.com/nager/Nager.Date/master/Src/Nager.Date/Model/PublicHoliday.cs.
    /// </summary>
    public class PublicHoliday
    {
        /// <summary>
        /// The date
        /// </summary>
        public DateTime Date { get; private set; }
        /// <summary>
        /// Local name
        /// </summary>
        public string LocalName { get; private set; }
        /// <summary>
        /// English name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// ISO 3166-1 alpha-2
        /// </summary>
        public CountryCode CountryCode { get; private set; }
        /// <summary>
        /// Is this public holiday every year on the same date
        /// </summary>
        public bool Fixed { get; private set; }
        /// <summary>
        /// Is this public holiday in every county (federal state)
        /// </summary>
        public bool Global { get { return this.Counties?.Length > 0 ? false : true; } }
        /// <summary>
        /// ISO-3166-2 - Federal states
        /// </summary>
        public string[] Counties { get; private set; }
        /// <summary>
        /// A list of types the public holiday it is valid
        /// </summary>
        public PublicHolidayType Type { get; private set; }
        /// <summary>
        /// The launch year of the public holiday
        /// </summary>
        public int? LaunchYear { get; private set; }

        /// <summary>
        /// Date and Name of the PublicHoliday
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.Date:yyyy-MM-dd} {this.Name}";
        }
    }
}