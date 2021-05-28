using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Holidays.Contracts;

namespace Holidays.Nager.Date
{
    /// <summary>
    /// Helper class to get holidays via Nager.Date with cached results.
    /// </summary>
    public class NagerDateCachedClient : IHolidaysProvider
    {
        private readonly NagerDateClient _client;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ConcurrentDictionary<string, CachedHolidays> _cache =
            new ConcurrentDictionary<string, CachedHolidays>();
        private static TimeSpan _cachePeriod = TimeSpan.FromHours(1);

        public NagerDateCachedClient(NagerDateClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<CountryHoliday>> GetHolidaysAsync(int year, string countryCode, CancellationToken cancellationToken)
        {
            var key = $"{year}-{countryCode}";
            if (_cache.TryGetValue(key, out var result) && !IsExpired(result))
                return result.Holidays;

            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_cache.TryGetValue(key, out result) && !IsExpired(result))
                    return result.Holidays;

                var holidays = await _client.GetHolidaysAsync(year, countryCode, cancellationToken).ConfigureAwait(false);
                _cache[key] = new CachedHolidays { Holidays = holidays };

                return holidays;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc/>
        public string[] GetSupportedCountries()
        {
            return _client.GetSupportedCountries();
        }

        private static bool IsExpired(CachedHolidays cached)
        {
            return cached.Added.Add(_cachePeriod) < DateTime.UtcNow;
        }

        private class CachedHolidays
        {
            public IReadOnlyCollection<CountryHoliday> Holidays { get; set; }

            public DateTime Added { get; } = new DateTime();
        }
    }
}
