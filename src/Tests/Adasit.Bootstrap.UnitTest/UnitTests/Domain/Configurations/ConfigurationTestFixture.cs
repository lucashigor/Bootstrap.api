namespace Adasit.Bootstrap.UnitTest.UnitTests.Domain.Configurations;

using System;
using Xunit;
using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.UnitTest.UnitTests;

public class ConfigurationTestFixture : UnitBaseFixture
{
    public Configuration GetValidConfiguration()
    {
        return new(
            GetStringRigthSize(5, 100),
            GetStringRigthSize(5, 300),
            GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddDays(1),
            DateTimeOffset.UtcNow.AddDays(15)
            );
    }

    [CollectionDefinition(nameof(ConfigurationTestFixture))]
    public class ConfigurationTestFixtureCollection : ICollectionFixture<ConfigurationTestFixture>
    {
    }
}
