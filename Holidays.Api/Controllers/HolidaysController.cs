using System;
using System.Threading;
using System.Threading.Tasks;
using Holidays.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Holidays.Api.Controllers
{
    /// <summary>
    /// Getting holidays and statistics.
    /// </summary>
    [ApiController]
    [Route("holidays")]
    public class HolidaysController : ControllerBase
    {
        private const string TagName = "Holidays";

        private readonly ILogger<HolidaysController> _logger;
        private readonly IHolidaysService _service;

        public HolidaysController(ILogger<HolidaysController> logger, IHolidaysService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Returns holidays for country in selected year.
        /// </summary>
        /// <response code="200">List of holidays</response>
        /// <response code="400">Provided data was incorrect</response>
        [HttpGet]
        [Route("{year}/{countryCode}")]
        [OpenApiTag(TagName)]
        [ProducesResponseType(typeof(HolidayList), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHolidays(int year, string countryCode, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get holidays for {countryCode} in {year}.");

            var result = await _service.GetHolidaysAsync(year, countryCode, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Returns country with the most holidays in selected year.
        /// </summary>
        /// <response code="200">Country code with holidays count</response>
        /// <response code="400">Provided data was incorrect</response>
        [HttpGet]
        [Route("{year}/most-holidays/country")]
        [OpenApiTag(TagName)]
        [ProducesResponseType(typeof(CountrySummary), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountryWithMostHolidaysAsync(int year, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get country with maximum number of holidays in {year}.");

            var result = await _service.GetCountryWithMostHolidaysAsync(year, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Returns month with the most holidays in selected year.
        /// </summary>
        /// <response code="200">Month with holidays count and list of countries having these holidays</response>
        /// <response code="400">Provided data was incorrect</response>
        [HttpGet]
        [Route("{year}/most-holidays/month")]
        [OpenApiTag(TagName)]
        [ProducesResponseType(typeof(MonthSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMonthWithMostHolidaysAsync(int year, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get month with maximum number of holidays in {year}.");

            var result = await _service.GetMonthWithMostHolidaysAsync(year, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Returns country with the most unique holidays in selected year.
        /// </summary>
        /// <response code="200">Country code with holidays count</response>
        /// <response code="400">Provided data was incorrect</response>
        [HttpGet]
        [Route("{year}/most-unique-holidays/country")]
        [OpenApiTag(TagName)]
        [ProducesResponseType(typeof(CountrySummary), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountryWithMostUniqueHolidaysAsync(int year, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get month with maximum number of holidays in {year}.");

            var result = await _service.GetCountryWithMostUniqueHolidaysAsync(year, cancellationToken);
            return Ok(result);
        }
    }
}
