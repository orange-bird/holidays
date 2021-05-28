using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Holidays.Contracts;
using Holidays.Core;
using Moq;
using NUnit.Framework;

namespace Holidays.Services.Tests
{
    [TestFixture]
    public class HolidaysServiceTests
    {
        private Mock<IHolidaysProvider> _providerMock;

        private Mock<HolidaysService> _serviceMock;

        private HolidaysService Service => _serviceMock.Object;

        [OneTimeSetUp]
        public void Setup()
        {
            _providerMock = new Mock<IHolidaysProvider>(MockBehavior.Strict);
            _serviceMock = new Mock<HolidaysService>(MockBehavior.Strict, _providerMock.Object);
        }

        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(1999)]
        public void GetCountryWithMostHolidays_ShouldThrow_OnUnsupportedYear(int year)
        {
            Assert.ThrowsAsync<BadArgumentException>(async () => await Service.GetCountryWithMostHolidaysAsync(year, CancellationToken.None));
        }

        [Test]
        public void GetHolidays_ShouldThrow_OnUnsupportedCountry()
        {
            _providerMock.Setup(x => x.IsCountrySupported(It.IsAny<string>())).Returns(false);
            Assert.ThrowsAsync<BadArgumentException>(async () => await Service.GetHolidaysAsync(DateTime.UtcNow.Year, "XX", CancellationToken.None));
        }

        [Test]
        public async Task GetMonthWithMostHolidays()
        {
            const string countryCode = "XX";
            _providerMock.Setup(x => x.GetSupportedCountries()).Returns(new string[] { countryCode });
            _providerMock.Setup(x => x.IsCountrySupported(countryCode)).Returns(true);
            _providerMock.Setup(x => x.GetHolidaysAsync(It.IsAny<int>(), countryCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CountryHoliday>
                {
                    new CountryHoliday { Date = new DateTime(2021, 1, 1), CountryCode = "XX" },
                    new CountryHoliday { Date = new DateTime(2021, 1, 2), CountryCode = "XX" },
                    new CountryHoliday { Date = new DateTime(2021, 1, 7), CountryCode = "XX" },
                    new CountryHoliday { Date = new DateTime(2021, 5, 1), CountryCode = "XX" },
                    new CountryHoliday { Date = new DateTime(2021, 5, 9), CountryCode = "XX" },
                    new CountryHoliday { Date = new DateTime(2021, 6, 12), CountryCode = "XX" }
                });
            var month = await Service.GetMonthWithMostHolidaysAsync(2021, CancellationToken.None);
            Assert.AreEqual(1, month.Month);
        }
    }
}
