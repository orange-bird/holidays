using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Holidays.Contracts;
using Holidays.Core;

namespace Holidays.Services
{
    /// <summary>
    /// Getting holidays.
    /// </summary>
    public class HolidaysService : IHolidaysService
    {
        private const int minimalSupportedYear = 2000;

        private readonly IHolidaysProvider _provider;

        public HolidaysService(IHolidaysProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc/>
        public async Task<HolidayList> GetHolidaysAsync(int year, string countryCode, CancellationToken cancellationToken)
        {
            if (year < minimalSupportedYear)
                throw new BadArgumentException($"Supported year is from {minimalSupportedYear}.", nameof(year)) { ParamValue = year.ToString() };

            if (!_provider.IsCountrySupported(countryCode))
                throw new BadArgumentException($"Country code {countryCode} is not supported", nameof(year)) { ParamValue = year.ToString() };

            var holidays = await _provider.GetHolidaysAsync(year, countryCode, cancellationToken).ConfigureAwait(false);
            return new HolidayList
            {
                CountryCode = countryCode,
                Holidays = holidays.Select(x => new Holiday { Date = x.Date, Name = x.Name }).ToArray()
            };
        }

        /// <inheritdoc/>
        public async Task<CountrySummary> GetCountryWithMostHolidaysAsync(int year, CancellationToken cancellationToken)
        {
            var holidays = await GetAllHolidaysAsync(year, cancellationToken).ConfigureAwait(false);
            var holidaysOrderedAndGroupedByCountry = holidays
                .GroupBy(x => x.CountryCode)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();
            return holidaysOrderedAndGroupedByCountry != null
                ? new CountrySummary
                {
                    CountryCode = holidaysOrderedAndGroupedByCountry.Key,
                    HolidayCount = holidaysOrderedAndGroupedByCountry.Count()
                }
                : null;
        }

        /// <inheritdoc/>
        public async Task<MonthSummary> GetMonthWithMostHolidaysAsync(int year, CancellationToken cancellationToken)
        {
            var holidays = await GetAllHolidaysAsync(year, cancellationToken).ConfigureAwait(false);
            var holidaysOrderedAndGroupedByMonth = holidays
                .GroupBy(x => x.Date.Month)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();
            return holidaysOrderedAndGroupedByMonth != null
                ? new MonthSummary
                {
                    Month = holidaysOrderedAndGroupedByMonth.Key,
                    CountryCodes = holidaysOrderedAndGroupedByMonth.Select(x => x.CountryCode).Distinct().ToArray(),
                    HolidayCount = holidaysOrderedAndGroupedByMonth.Count()
                }
                : null;
        }

        /// <inheritdoc/>
        public async Task<CountrySummary> GetCountryWithMostUniqueHolidaysAsync(int year, CancellationToken cancellationToken)
        {
            var holidays = await GetAllHolidaysAsync(year, cancellationToken).ConfigureAwait(false);
            var holidaysOrderedAndGroupedByDays = holidays
                .GroupBy(x => x.Date)
                .Where(x => x.Count() == 1)
                .SelectMany(x => x)
                .GroupBy(x => x.CountryCode)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();
            return holidaysOrderedAndGroupedByDays != null
                ? new CountrySummary
                {
                    CountryCode = holidaysOrderedAndGroupedByDays.Key,
                    HolidayCount = holidaysOrderedAndGroupedByDays.Count()
                }
                : null;
        }

        private async Task<IReadOnlyCollection<CountryHoliday>> GetAllHolidaysAsync(int year, CancellationToken cancellationToken)
        {
            if (year < minimalSupportedYear)
                throw new BadArgumentException($"Supported year is from {minimalSupportedYear}.", nameof(year)) { ParamValue = year.ToString() };

            var supportedCountries = _provider.GetSupportedCountries() ?? new string[0];
            if (supportedCountries.Length == 0)
                return new CountryHoliday[0];

            var tasks = supportedCountries.Select(x => _provider.GetHolidaysAsync(year, x, cancellationToken)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            var result = new List<CountryHoliday>();

            foreach (var task in tasks)
                result.AddRange(await task.ConfigureAwait(false));

            return result;
        }
    }
}
