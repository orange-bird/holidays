using System;
using System.Threading;
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
        public void GetCustomers_ShouldThrow_OnUnsupportedYear(int year)
        {
            Assert.ThrowsAsync<BadArgumentException>(async () => await Service.GetCountryWithMostHolidaysAsync(year, CancellationToken.None));
        }

        [Test]
        public void GetCustomer_ShouldThrow_OnUnsupportedCountry()
        {
            _providerMock.Setup(x => x.GetSupportedCountries()).Returns(new string[0]);
            Assert.ThrowsAsync<BadArgumentException>(async () => await Service.GetHolidaysAsync(DateTime.UtcNow.Year, "XX", CancellationToken.None));
        }
    }
}
