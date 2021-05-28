using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Holidays.Contracts
{
    /// <summary>
    /// Helper class to get holidays via Nager.Date.
    /// </summary>
    public interface IHolidaysProvider
    {
        /// <summary>
        /// Returns holidays for selected year/country pair.
        /// </summary>
        Task<IReadOnlyCollection<CountryHoliday>> GetHolidaysAsync(int year, string countryCode, CancellationToken cancellationToken);

        /// <summary>
        /// Returns list of supported country codes.
        /// </summary>
        string[] GetSupportedCountries();

        /// <summary>
        /// Returns whether specified country code is supported.
        /// </summary>
        bool IsCountrySupported(string countryCode);
    }
}
