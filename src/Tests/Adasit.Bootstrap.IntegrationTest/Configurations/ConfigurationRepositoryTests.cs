using Adasit.Bootstrap.Infrastructure.Repositories;
using Adasit.Bootstrap.Infrastructure.Repositories.Context;
using FluentAssertions;

namespace Adasit.Bootstrap.IntegrationTest.Configurations;

[Collection(nameof(ConfigurationTestFixture))]
public class ConfigurationRepositoryTests
{
    private readonly ConfigurationTestFixture fixture;

    public ConfigurationRepositoryTests(ConfigurationTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact(DisplayName = nameof(InsertNewConfigurationAsync))]
    [Trait("Integration", "Configuration - Access to a database")]
    public async void InsertNewConfigurationAsync()
    {
        //Arrange
        TestsUtil.BaseFixture.CreateDatabase();

        using PrincipalContext context = new(TestsUtil.BaseFixture.DbContextOptions!);

        var app = new ConfigurationRepository(context);

        var item = fixture.GetValidConfiguration();

        context.Configuration.RemoveRange(context.Configuration);

        //Act
        await app.Insert(item, CancellationToken.None);

        //Assert
        using var context2 = new PrincipalContext(TestsUtil.BaseFixture.DbContextOptions!);

        var database = context2.Configuration.Find(item.Id);

        database.Should().BeNull();

        context.SaveChanges();

        database = context2.Configuration.Find(item.Id);

        database.Should().NotBeNull();
    }

    [Fact(DisplayName = nameof(UpdateConfigurationAsync))]
    [Trait("Integration", "Configuration - Access to a database")]
    public async void UpdateConfigurationAsync()
    {
        //Arrange
        TestsUtil.BaseFixture.CreateDatabase();

        var item = fixture.GetValidConfiguration();

        using PrincipalContext context = new(TestsUtil.BaseFixture.DbContextOptions!);

        context.Configuration.Add(item);
        context.SaveChanges();

        var app = new ConfigurationRepository(context);
        var unitWork = new UnitOfWork(context);

        var oldDescription = item.Description;
        var newDescription = fixture.GetStringRigthSize(10, 100);

        //Act
        item.Modify(
            item.Name,
            item.Value,
            newDescription,
            item.StartDate,
            item.FinalDate);

        await app.Update(item, CancellationToken.None);

        //Assert
        using (var context2 = new PrincipalContext(TestsUtil.BaseFixture.DbContextOptions!))
        {
            var database1 = context2.Configuration.Find(item.Id);
            database1?.Description.Should().Be(oldDescription);
        }

        await unitWork.CommitAsync(CancellationToken.None);

        using (var context2 = new PrincipalContext(TestsUtil.BaseFixture.DbContextOptions!))
        {
            var database1 = context2.Configuration.Find(item.Id);
            database1?.Description.Should().Be(newDescription);
        }
    }

    [Fact(DisplayName = nameof(DeleteConfigurationAsync))]
    [Trait("Integration", "Configuration - Access to a database")]
    public async void DeleteConfigurationAsync()
    {
        //Arrange
        TestsUtil.BaseFixture.CreateDatabase();

        var item = fixture.GetValidConfiguration();

        using PrincipalContext context = new(TestsUtil.BaseFixture.DbContextOptions!);

        context.Configuration.Add(item);
        context.SaveChanges();

        var unitWork = new UnitOfWork(context);

        var app = new ConfigurationRepository(context);

        //Act
        await app.Delete(item, CancellationToken.None);

        //Assert
        var database = context.Configuration.Find(item.Id);

        database.Should().NotBeNull();

        await unitWork.CommitAsync(CancellationToken.None);

        database = context.Configuration.Find(item.Id);

        database.Should().BeNull();
    }

    [Fact(DisplayName = nameof(GetByIdConfigurationAsync))]
    [Trait("Integration", "Configuration - Access to a database")]
    public async void GetByIdConfigurationAsync()
    {
        //Arrange
        TestsUtil.BaseFixture.CreateDatabase();

        var item = fixture.GetValidConfiguration();

        using PrincipalContext context = new(TestsUtil.BaseFixture.DbContextOptions!);

        context.Configuration.Add(item);
        context.SaveChanges();

        var app = new ConfigurationRepository(context);

        //Act
        var database = await app.GetById(item.Id, CancellationToken.None);

        //Assert
        database.Should().NotBeNull();
    }
}