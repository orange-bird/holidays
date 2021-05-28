using System;
using System.Collections.Generic;

namespace Holidays.Core
{
    /// <summary>
    /// Occurs when argument supplied to method was incorrect.
    /// </summary>
    [Serializable]
    public class BadArgumentException : Exception
    {
        /// <summary>
        /// Name of parameter which caused an exception
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// Value of parameter which caused an exception
        /// </summary>
        public string ParamValue { get; set; }

        public BadArgumentException(string message, string paramName)
            : base(message)
        {
            ParamName = paramName;
        }

        public BadArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Returns detailed information about error.
        /// </summary>
        public Dictionary<string, object> GetAsDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(ParamName))
            {
                dictionary.Add(nameof(ParamName), ParamName);
                dictionary.Add(nameof(ParamValue), ParamValue);
            }

            return dictionary;
        }
    }
}
