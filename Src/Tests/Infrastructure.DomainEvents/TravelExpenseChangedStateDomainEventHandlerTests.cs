using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.Infrastructure.DomainEvents
{
    public class TravelExpenseChangedStateDomainEventHandlerIntegrationTests
    {
        [Test]
        public async Task HandleAsync_()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var repository = testContext.GetUnitOfWork().Repository;
                var stageEntities = repository.List<StageEntity>();
                var userEntities = repository.List<UserEntity>();
                var stageBefore = stageEntities.Single(x=>x.Value==TravelExpenseStage.ReportedDone);
                var userEntityBob = userEntities.Single(x => x.Subject == TestData.DummySekSubBob);
                var travelExpenseChangedStateDomainEvent = new TravelExpenseChangedStateDomainEvent(stageBefore,testContext.TravelExpenseEntity1, userEntityBob, "http://nowhere.com");
                var sut = testContext.ServiceProvider.GetRequiredService<IHandle<TravelExpenseChangedStateDomainEvent>>();

                // Act
                await sut.HandleAsync(travelExpenseChangedStateDomainEvent);

                // Assert
                var memoryListMessageSenderService = testContext
                    .ServiceProvider
                    .GetServices(typeof(IMessageSenderService))
                    .Single(x=>x.GetType()==typeof(MemoryListMessageSenderService)) as MemoryListMessageSenderService;
                var messages = memoryListMessageSenderService.MessagesSend;
                Assert.That(messages.Count, Is.EqualTo(2));
                foreach (var message in messages)
                {
                    Assert.That((message.Value[1] as UserEntity).Subject == TestData.DummyPolSubAlice);//receiver is alice
                }
            }
        }
    }
}
