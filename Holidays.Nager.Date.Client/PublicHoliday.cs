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
        public DateTime Date { get; set; }
        /// <summary>
        /// Local name
        /// </summary>
        public string LocalName { get; set; }
        /// <summary>
        /// English name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ISO 3166-1 alpha-2
        /// </summary>
        public CountryCode CountryCode { get; set; }
        /// <summary>
        /// Is this public holiday every year on the same date
        /// </summary>
        public bool Fixed { get; set; }
        /// <summary>
        /// Is this public holiday in every county (federal state)
        /// </summary>
        public bool Global { get { return (Counties?.Length ?? 0) == 0; } }
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
        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} {Name}";
        }
    }
}