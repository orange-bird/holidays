using System;

namespace Holidays.Contracts
{
    /// <summary>
    /// Holiday.
    /// </summary>
    public class Holiday
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// English name
        /// </summary>
        public string Name { get; set; }
    }
}
