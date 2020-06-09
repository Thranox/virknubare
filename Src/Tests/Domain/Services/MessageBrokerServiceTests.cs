using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.TestHelpers;

namespace Tests.Domain.Services
{
    public class MessageBrokerServiceIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SendMessageAsync_KeyForAssignedForPayment_ReturnsTrue()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var userEntityAnders = new UserEntity("Anders", Guid.NewGuid().ToString());
                var receivingUserEntities = new[] {userEntityAnders};
                var customerEntity = testContext.GetDummyCustomer1();
                var stageEntity = testContext.GetStages().Single(x=>x.Value==TravelExpenseStage.Initial);
                var travelExpenseEntity = new TravelExpenseEntity("Description", userEntityAnders, customerEntity, stageEntity);

                var sut = testContext.ServiceProvider.GetRequiredService<IMessageBrokerService>();

                // Act
                var actual = await sut.SendMessageAsync(receivingUserEntities, MessageKind.YourTravelExpenseHasChangedState, new IMessageValueEnricher[] { travelExpenseEntity, customerEntity });

                // Assert
                Assert.That(actual, Is.EqualTo(1));
            }
        }
    }
}