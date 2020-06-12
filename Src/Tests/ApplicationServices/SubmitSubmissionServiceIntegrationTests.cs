using System;
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
        //[Explicit("Awaits 2020-27")]
        public async Task SubmitAsync_NoParameters_SubmitsAll()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var path = Path.GetTempFileName();
                File.WriteAllText(path, "Gibberish!");
                SubmissionEntity entity = new SubmissionEntity() { PathToFile = path };
                testContext.GetUnitOfWork().Repository.Add(entity);
                await testContext.GetUnitOfWork().CommitAsync();
                
                var sut = testContext.ServiceProvider.GetRequiredService<ISubmitSubmissionService>();

                // Act
                await sut.SubmitAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                // Assert
                var submissionEntity = testContext.GetUnitOfWork().Repository.GetById<SubmissionEntity>(entity.Id);
                Assert.That(submissionEntity.SubmissionTime, Is.Not.Null);
                Assert.That(DateTime.Now - submissionEntity.SubmissionTime, Is.LessThan(TimeSpan.FromSeconds(10)));
            }
        }
    }
}