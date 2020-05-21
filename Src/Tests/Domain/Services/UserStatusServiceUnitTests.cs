using System;
using Domain.Services;
using Domain.ValueObjects;
using NUnit.Framework;

namespace Tests.Domain.Services
{
    public class UserStatusServiceUnitTests
    {
        [TestCase(UserStatus.Initial)]
        [TestCase(UserStatus.Registered)]
        [TestCase(UserStatus.UserAdministrator)]
        public void GetUserStatusFromString_StringVersionOfExistingStatus_ReturnsCorrectStatus(
            UserStatus expectedUserStatus)
        {
            // Arrange
            var userStatusString = expectedUserStatus.ToString();
            var sut = new UserStatusService();

            // Act
            var actual = sut.GetUserStatusFromString(userStatusString);

            // Assert
            Assert.That(actual, Is.EqualTo(expectedUserStatus));
        }

        [Test]
        public void GetUserStatusFromString_StringVersionOfExistingStatus_ReturnsCorrectStatus1()
        {
            // Arrange
            var userStatusString = "abcdef";
            var sut = new UserStatusService();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => sut.GetUserStatusFromString(userStatusString));
        }

        [TestCase(UserStatus.Initial)]
        [TestCase(UserStatus.Registered)]
        [TestCase(UserStatus.UserAdministrator)]
        public void GetUserStatusFromInt_StringVersionOfExistingStatus_ReturnsCorrectStatus(
            UserStatus expectedUserStatus)
        {
            // Arrange
            var userStatusInt = (int) expectedUserStatus;
            var sut = new UserStatusService();

            // Act
            var actual = sut.GetUserStatusFromInt(userStatusInt);

            // Assert
            Assert.That(actual, Is.EqualTo(expectedUserStatus));
        }

        [Test]
        public void GetUserStatusFromInt_StringVersionOfExistingStatus_ReturnsCorrectStatus1()
        {
            // Arrange
            var userStatusInt = 42;
            var sut = new UserStatusService();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => sut.GetUserStatusFromInt(userStatusInt));
        }
    }
}