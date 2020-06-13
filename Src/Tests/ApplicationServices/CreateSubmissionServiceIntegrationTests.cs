using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using TestHelpers;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class CreateSubmissionServiceIntegrationTests
    {
        [Test]
        [Explicit("Awaits 2020-26")]
        public async Task GetAsync_NoParameters_ReturnsFlowSteps()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<ICreateSubmissionService>();

                // Act
                var actual = await sut.CreateAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                // Assert
                Assert.That(actual, Is.Not.Null);
            }
        }
    }
}