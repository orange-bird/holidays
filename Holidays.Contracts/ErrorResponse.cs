using System.Collections.Generic;

namespace Holidays.Contracts
{
    /// <summary>
    /// Error response.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Http code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details
        /// </summary>
        public Dictionary<string, object> Context { get; set; }
    }
}
