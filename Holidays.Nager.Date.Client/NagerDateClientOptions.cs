﻿using System;

namespace Holidays.Nager.Date
{
    /// <summary>
    /// Options for Nager.Date client.
    /// </summary>
    public class NagerDateClientOptions
    {
        /// <summary>
        /// API base address
        /// </summary>
        public Uri ApiEndpoint { get; set; } = new Uri("https://date.nager.at/");

        /// <summary>
        /// Codes of API supported countries
        /// </summary>
        public string[] SupportedCountries { get; set; } = new string[]
        {
            "AD", "AR", "AT", "AU", "AX",
            "BB", "BE", "BG", "BO", "BR", "BS", "BW", "BY", "BZ",
            "CA", "CH", "CL", "CN", "CO", "CR", "CU", "CY", "CZ",
            "DE", "DK", "DO",
            "EC", "EE", "EG", "ES",
            "FI", "FO", "FR",
            "GA", "GB", "GD", "GL", "GR", "GT", "GY",
            "HN", "HR", "HT", "HU",
            "IE", "IM", "IS", "IT",
            "JE", "JM",
            "LI", "LS", "LT", "LU", "LV",
            "MA", "MC", "MD", "MG", "MK", "MT", "MX", "MZ",
            "NA", "NI", "NL", "NO", "NZ",
            "PA", "PE", "PL", "PR", "PT", "PY",
            "RO", "RS", "RU",
            "SE", "SI", "SJ", "SK", "SM", "SR", "SV",
            "TN", "TR",
            "UA", "US", "UY",
            "VA", "VE",
            "ZA"
        };
    }
}
