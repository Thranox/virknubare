using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestHelpers;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class GetStatisticsServiceIntegrationTests
    {
        [Test]
        [Explicit("Awaits 2020-46")]
        public async Task GetAsync_NoParameters_ReturnsFlowSteps()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetStatisticsService>();

                // Act
                var actual = await sut.GetAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                // Assert
                Assert.That(actual, Is.Not.Null);
            }
        }
    }
}