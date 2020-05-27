using System.IO;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class SubmissionControllerIntegrationTests
    {
        [Test]
        [Explicit("Awaits 2020-26")]
        public async Task Get_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                testContext.SetCallingUserBySub(TestData.DummyPolSubAlice);
                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Post();

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as SubmissionPostResponse;
                Assert.That(value, Is.Not.Null);
            }
        }

        [Test]
        [Explicit("Awaits 2020-27")]
        public async Task PostTbd_NoParameters_SendsSubmissions()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                testContext.SetCallingUserBySub(TestData.DummyPolSubAlice);

                var path = Path.GetTempFileName();
                File.WriteAllText(path, "Gibberish!");
                testContext.GetUnitOfWork().Repository.Add(new SubmissionEntity {PathToFile = path});
                await testContext.GetUnitOfWork().CommitAsync();

                var sut = GetSut(testContext);

                // Act
                var actual = await sut.PostTbd();

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as SubmissionPostResponse;
                Assert.That(value, Is.Not.Null);
            }
        }

        private SubmissionController GetSut(IntegrationTestContext testContext)
        {
            return new SubmissionController(
                testContext.ServiceProvider.GetRequiredService<ISubManagementService>(),
                testContext.ServiceProvider.GetRequiredService<ICreateSubmissionService>(),
                testContext.ServiceProvider.GetRequiredService<ISubmitSubmissionService>());
        }
    }
}