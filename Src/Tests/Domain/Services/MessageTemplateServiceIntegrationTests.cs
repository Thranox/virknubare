using System;
using System.Linq;
using Domain.Interfaces;
using Domain.ValueObjects;
using IdentityServer4.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.TestHelpers;

namespace Tests.Domain.Services
{
    public class MessageTemplateServiceIntegrationTests
    {
        [Test]
        public void Get_AllValuesExceptNone_ReturnsTemplate()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetRequiredService<IMessageTemplateService>();
                foreach (var messageKind in ((MessageKind[])Enum.GetValues(typeof(MessageKind))).Where(x=>x!= MessageKind.None))
                {
                    // Act
                    var actual = sut.Get(messageKind);

                    // Assert
                    Assert.That(actual.Body.IsNullOrEmpty(), Is.False);
                    Assert.That(actual.Subject.IsNullOrEmpty(), Is.False);
                }
            }
        }
    }
}