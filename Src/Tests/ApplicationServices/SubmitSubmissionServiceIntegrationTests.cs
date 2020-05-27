using System.IO;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class SubmitSubmissionServiceIntegrationTests
    {
        [Test]
        [Explicit("Awaits 2020-27")]
        public async Task SubmitAsync_NoParameters_SubmitsAll()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var path = Path.GetTempFileName();
                File.WriteAllText(path,"Gibberish!");
                testContext.GetUnitOfWork().Repository.Add(new SubmissionEntity() {PathToFile = path});
                await testContext.GetUnitOfWork().CommitAsync();
                
                var sut = testContext.ServiceProvider.GetRequiredService<ISubmitSubmissionService>();

                // Act
                var actual = await sut.SubmitAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                // Assert
                Assert.That(actual, Is.Not.Null);
            }
        }
    }
}