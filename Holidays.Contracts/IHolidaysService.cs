using System.Threading;
using System.Threading.Tasks;

namespace Holidays.Contracts
{
    /// <summary>
    /// Customer-related public methods.
    /// </summary>
    public interface IHolidaysService
    {
        /// <summary>
        /// Returns holidays for selected year/country pair.
        /// </summary>
        Task<HolidayList> GetHolidaysAsync(int year, string countryCode, CancellationToken cancellationToken);

        /// <summary>
        /// Returns country with most holidays in selected year.
        /// </summary>
        Task<CountrySummary> GetCountryWithMostHolidaysAsync(int year, CancellationToken cancellationToken);

        /// <summary>
        /// Returns month having most holidays in selected year.
        /// </summary>
        Task<MonthSummary> GetMonthWithMostHolidaysAsync(int year, CancellationToken cancellationToken);

        /// <summary>
        /// Returns country having most unique holidays in selected year.
        /// </summary>
        Task<CountrySummary> GetCountryWithMostUniqueHolidaysAsync(int year, CancellationToken cancellationToken);
    }
}
