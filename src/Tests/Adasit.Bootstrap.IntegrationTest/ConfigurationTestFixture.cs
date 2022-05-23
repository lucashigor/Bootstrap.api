namespace Adasit.Bootstrap.IntegrationTest;

using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.TestsUtil;
using System;

public class ConfigurationTestFixture : BaseFixture
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