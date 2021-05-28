using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Holidays.Nager.Date.Tests
{
    [TestFixture]
    public class NagerDateClientTests
    {
        private Mock<IMockHttpMessageHandler> _handlerMock;
        private NagerDateClient _client;
        private NagerDateClientOptions _clientOptions = new NagerDateClientOptions
        {
            SupportedCountries = new string[] { "NL", "RU" },
            MaxRequestParallelism = MaxRequests
        };
        private const int MaxRequests = 5;

        [OneTimeSetUp]
        public void Setup()
        {
            _handlerMock = new Mock<IMockHttpMessageHandler>(MockBehavior.Strict);
            var handler = new MockHttpMessageHandler(_handlerMock.Object);
            _client = new NagerDateClient(new HttpClient(handler), _clientOptions);
        }

        [Test]
        public async Task GetHolidays_ShouldThrottle()
        {
            var processed = 0;
            var current = 0;
            var maximum = 0;
            _handlerMock
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    Interlocked.Increment(ref processed);
                    var actual = Interlocked.Increment(ref current);
                    if (actual > MaxRequests)
                        throw new Exception($"Should throttle at maximum of {MaxRequests} simultaneous requests");

                    InterlockedMax(ref maximum, actual);
                })
                .Returns(async () =>
                {
                    Interlocked.Decrement(ref current);
                    await Task.Delay(100);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var startingYear = 2001;
            var numberOfYears = 100;
            var tasks = Enumerable.Range(startingYear, numberOfYears)
                .Select(x => _client.GetHolidaysAsync(x, "RU", CancellationToken.None))
                .ToArray();
            await Task.WhenAll(tasks);

            CollectionAssert.IsEmpty(tasks.Where(x => !x.IsCompletedSuccessfully));

            Assert.AreEqual(processed, numberOfYears, $"Should process all {numberOfYears} but only {processed} were processes");

            Assert.That(maximum, Is.GreaterThan(1), "Should make requests in parallel");
            Console.WriteLine($"Maximum {maximum} of {MaxRequests} simultaneous requests was reached");
        }

        public static int InterlockedMax(ref int location, int value)
        {
            int initialValue, newValue;
            do
            {
                initialValue = location;
                newValue = Math.Max(initialValue, value);
            }
            while (Interlocked.CompareExchange(ref location, newValue, initialValue) != initialValue);
            return initialValue;
        }

        public interface IMockHttpMessageHandler
        {
            Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        }

        public class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly IMockHttpMessageHandler _realMockHandler;

            public MockHttpMessageHandler(IMockHttpMessageHandler realMockHandler)
            {
                _realMockHandler = realMockHandler;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return await _realMockHandler.SendAsync(request, cancellationToken);
            }
        }
    }
}
