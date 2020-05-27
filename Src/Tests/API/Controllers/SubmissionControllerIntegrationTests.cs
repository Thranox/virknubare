using System.Threading.Tasks;
using API.Shared.Controllers;
using Application.Dtos;
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
                //testContext.SetCallingUserBySub(TestData.DummyPolSubAlice);
                var sut = testContext.ServiceProvider.GetService<SubmissionController>();

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
    }
}