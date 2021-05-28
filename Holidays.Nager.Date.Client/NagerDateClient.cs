using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Holidays.Contracts;
using Newtonsoft.Json;

namespace Holidays.Nager.Date
{
    /// <summary>
    /// Helper class to get holidays via Nager.Date.
    /// </summary>
    public class NagerDateClient : IHolidaysProvider
    {
        private const int MaxRequestParallelism = 5;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(MaxRequestParallelism, MaxRequestParallelism);

        private readonly HttpClient _httpClient;
        private readonly Uri _apiEndpoint;
        private readonly string[] _supportedCountryCodes;

        public NagerDateClient(HttpClient httpClient, NagerDateClientOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiEndpoint = options?.ApiEndpoint ?? throw new ArgumentNullException(nameof(options));
            _supportedCountryCodes = options.SupportedCountries;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<CountryHoliday>> GetHolidaysAsync(int year, string countryCode, CancellationToken cancellationToken)
        {
            if (_supportedCountryCodes?.Contains(countryCode) != true)
                throw new ArgumentException($"Unsupported country code {countryCode}", nameof(countryCode));

            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                var uri = new Uri(_apiEndpoint, $"api/v3/PublicHolidays/{year}/{countryCode}");
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                using (var response = await _httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    using (var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        var deserializer = JsonSerializer.CreateDefault();
                        using (var sr = new StreamReader(contentStream))
                        using (var jtr = new JsonTextReader(sr))
                        {
                            return deserializer.Deserialize<PublicHoliday[]>(jtr)
                                .Select(x => new CountryHoliday
                                {
                                    CountryCode = countryCode,
                                    Date = x.Date,
                                    Name = x.Name
                                })
                                .ToList();
                        }
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc/>
        public string[] GetSupportedCountries()
        {
            return _supportedCountryCodes;
        }
    }
}
