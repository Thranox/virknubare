using System;
using System.Reflection;
using NUnit.Framework;
using SharedWouldBeNugets;

namespace Tests.SharedWouldBeNugets
{
    public class BuildVersionInfoProviderTests
    {
        [Test]
        public void Test()
        {
            // Arrange

            // Act
            var buildVersionInfoString = BuildVersionInfoProvider.GetBuildVersionInfoString(Assembly.GetAssembly(typeof(BuildVersionInfoProviderTests)));

            // Assert
            Console.WriteLine("buildVersionInfoString:"+ buildVersionInfoString);
        }
    }
}