namespace Adasit.Bootstrap.UnitTest.UnitTests.Domain.Configurations;

using System;
using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using DomainEntity = Bootstrap.Domain.Entity;

[Collection(nameof(ConfigurationTestFixture))]
public class ConfigurationTests
{
    private readonly ConfigurationTestFixture fixture;

    public ConfigurationTests(ConfigurationTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instatiate))]
    [Trait("Domain", "Configuration - Aggregates")]
    public void Instatiate()
    {
        //Act
        var datetimeBefore = DateTime.UtcNow;

        var validData = fixture.GetValidConfiguration();

        var datetimeAfter = DateTime.UtcNow.AddSeconds(1);

        //Assert
        validData.Should().NotBeNull();
        validData.Id.Should().NotBeEmpty();
        validData.LastUpdateAt.Should().BeNull();
        validData.DeletedAt.Should().BeNull();
        (validData.CreatedAt > datetimeBefore).Should().BeTrue();
        (validData.CreatedAt < datetimeAfter).Should().BeTrue();
        (validData.StartDate > datetimeBefore).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(InstatiateRequeridFieldNameNull))]
    [Trait("Domain", "Configuration - Aggregates")]
    public void InstatiateRequeridFieldNameNull()
    {
        //Arrange
        var validData = fixture.GetValidConfiguration();

        //Act
        Action action =
            () => new DomainEntity.Configuration(null!,
            validData.Value,
            validData.Description,
            validData.StartDate,
            validData.FinalDate);

        //Assert
        var msg = $"{(int)ErrorsCodes.Validation}:{ErrorsMessages.NotNull.GetMessage(nameof(DomainEntity.Configuration.Name))}";

        action.Should().Throw<EntityGenericException>()
            .WithMessage(msg);
    }

    [Fact(DisplayName = nameof(InstatiateRequeridFieldValueNull))]
    [Trait("Domain", "Configuration - Aggregates")]
    public void InstatiateRequeridFieldValueNull()
    {
        //Arrange
        var validData = fixture.GetValidConfiguration();

        //Act
        Action action =
            () => new DomainEntity.Configuration(validData.Name,
            null!,
            validData.Description,
            validData.StartDate,
            validData.FinalDate);

        //Assert
        var msg = $"{(int)ErrorsCodes.Validation}:{ErrorsMessages.NotNull.GetMessage(nameof(DomainEntity.Configuration.Value))}";

        action.Should().Throw<EntityGenericException>()
            .WithMessage(msg);
    }

    [Fact(DisplayName = nameof(InstatiateRequeridFieldDescriptionNull))]
    [Trait("Domain", "Configuration - Aggregates")]
    public void InstatiateRequeridFieldDescriptionNull()
    {
        //Arrange
        var validData = fixture.GetValidConfiguration();

        //Act
        Action action =
            () => new DomainEntity.Configuration(validData.Name,
            validData.Value,
            null!,
            validData.StartDate,
            validData.FinalDate);

        //Assert
        var msg = $"{(int)ErrorsCodes.Validation}:{ErrorsMessages.NotNull.GetMessage(nameof(DomainEntity.Configuration.Description))}";

        action.Should().Throw<EntityGenericException>()
            .WithMessage(msg);
    }

    [Fact(DisplayName = nameof(InstatiateRequeridFieldStartDateNull))]
    [Trait("Domain", "Configuration - Aggregates")]
    public void InstatiateRequeridFieldStartDateNull()
    {
        //Arrange
        var validData = fixture.GetValidConfiguration();

        //Act
        Action action =
            () => new DomainEntity.Configuration(validData.Name,
            validData.Value,
            validData.Description,
            DateTimeOffset.MinValue,
            validData.FinalDate);

        //Assert
        var msg = $"{(int)ErrorsCodes.Validation}:{ErrorsMessages.NotDefaultDateTime.GetMessage(nameof(DomainEntity.Configuration.StartDate))}";

        action.Should().Throw<EntityGenericException>()
            .WithMessage(msg);
    }

    [Fact(DisplayName = nameof(InstatiateRequeridFieldFinalDateNull))]
    [Trait("Domain", "Configuration - Aggregates")]
    public void InstatiateRequeridFieldFinalDateNull()
    {
        //Arrange
        var validData = fixture.GetValidConfiguration();

        //Act
        Action action =
            () => new DomainEntity.Configuration(validData.Name,
            validData.Value,
            validData.Description,
            validData.StartDate,
            DateTimeOffset.MinValue);

        //Assert
        var msg = $"{(int)ErrorsCodes.Validation}:{ErrorsMessages.NotDefaultDateTime.GetMessage(nameof(DomainEntity.Configuration.FinalDate))}";

        action.Should().Throw<EntityGenericException>()
            .WithMessage(msg);
    }
}
