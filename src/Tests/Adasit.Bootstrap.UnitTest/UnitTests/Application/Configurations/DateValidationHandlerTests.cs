namespace Adasit.Bootstrap.UnitTest.UnitTests.Application.Configurations;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.Domain.Repository;
using Adasit.Bootstrap.UnitTest.UnitTests.Domain.Configurations;
using FluentAssertions;
using Moq;
using Xunit;

[Collection(nameof(ConfigurationTestFixture))]
public class DateValidationHandlerTests
{
    private readonly ConfigurationTestFixture fixture;
    private readonly Mock<IConfigurationRepository> configurationMock;
    private readonly Mock<Notifier> notifier;

    public DateValidationHandlerTests(ConfigurationTestFixture fixture)
    {
        this.fixture = fixture;
        this.configurationMock = new Mock<IConfigurationRepository>();
        this.notifier = new Mock<Notifier>();
    }

    [Fact(DisplayName = nameof(HandleDatesWithEmptyDatabaseAsync))]
    [Trait("Domain", "Configuration - DateValidationHandler")]
    public async Task HandleDatesWithEmptyDatabaseAsync()
    {
        var validData = fixture.GetValidConfiguration();

        var app = new DateValidationHandler(configurationMock.Object, notifier.Object);

        //Act
        await app.Handle(validData, CancellationToken.None);

        //Assert
        notifier.Object.Erros.Should().BeEmpty();
    }

    [Fact(DisplayName = nameof(HandleDatesWithSameNameClosedBeforeDatabaseAsync))]
    [Trait("Domain", "Configuration - DateValidationHandler")]
    public async Task HandleDatesWithSameNameClosedBeforeDatabaseAsync()
    {
        //close befor
        var validData = fixture.GetValidConfiguration();

        var beforeConfig = new Configuration(validData.Name, validData.Value, validData.Description, validData.StartDate.AddDays(-30), validData.StartDate.AddDays(-10));

        configurationMock.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Configuration>()
        {
            beforeConfig
        });

        var app = new DateValidationHandler(configurationMock.Object, notifier.Object);

        //Act
        await app.Handle(validData, CancellationToken.None);

        //Assert
        notifier.Object.Erros.Should().BeEmpty();
    }

    [Fact(DisplayName = nameof(HandleDatesWithSameNameStartsAfterDatabaseAsync))]
    [Trait("Domain", "Configuration - DateValidationHandler")]
    public async Task HandleDatesWithSameNameStartsAfterDatabaseAsync()
    {
        //open after
        var validData = fixture.GetValidConfiguration();

        var beforeConfig = new Configuration(validData.Name, validData.Value, validData.Description, validData.FinalDate.AddDays(10), validData.FinalDate.AddDays(20));

        configurationMock.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Configuration>()
        {
            beforeConfig
        });

        var app = new DateValidationHandler(configurationMock.Object, notifier.Object);

        //Act
        await app.Handle(validData, CancellationToken.None);

        //Assert
        notifier.Object.Erros.Should().BeEmpty();
    }

    [Fact(DisplayName = nameof(HandleDatesWithSameNameOpeningDuringDatabaseAsync))]
    [Trait("Domain", "Configuration - DateValidationHandler")]
    public async Task HandleDatesWithSameNameOpeningDuringDatabaseAsync()
    {
        //open during
        var validData = fixture.GetValidConfiguration();

        var beforeConfig = new Configuration(validData.Name, validData.Value, validData.Description, validData.StartDate.AddDays(-3), validData.StartDate.AddDays(1));

        configurationMock.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Configuration>()
        {
            beforeConfig
        });

        var app = new DateValidationHandler(configurationMock.Object, notifier.Object);

        //Act
        await app.Handle(validData, CancellationToken.None);

        //Assert
        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationStartDate().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationStartDate().Message);
    }

    [Fact(DisplayName = nameof(HandleDatesWithSameNameClosingDuringDatabaseAsync))]
    [Trait("Domain", "Configuration - DateValidationHandler")]
    public async Task HandleDatesWithSameNameClosingDuringDatabaseAsync()
    {
        //close during
        var validData = fixture.GetValidConfiguration();

        var beforeConfig = new Configuration(validData.Name, validData.Value, validData.Description, validData.StartDate.AddDays(3), validData.FinalDate.AddDays(1));

        configurationMock.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Configuration>()
        {
            beforeConfig
        });

        var app = new DateValidationHandler(configurationMock.Object, notifier.Object);

        //Act
        await app.Handle(validData, CancellationToken.None);

        //Assert
        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationEndDate().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationEndDate().Message);
    }

    [Fact(DisplayName = nameof(HandleDatesWithSameNameDuringDatabaseAsync))]
    [Trait("Domain", "Configuration - DateValidationHandler")]
    public async Task HandleDatesWithSameNameDuringDatabaseAsync()
    {
        //open and close during - inside
        var validData = fixture.GetValidConfiguration();

        var beforeConfig = new Configuration(validData.Name, validData.Value, validData.Description, validData.StartDate.AddDays(-3), validData.FinalDate.AddDays(3));

        configurationMock.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Configuration>()
        {
            beforeConfig
        });

        var app = new DateValidationHandler(configurationMock.Object, notifier.Object);

        //Act
        await app.Handle(validData, CancellationToken.None);

        //Assert
        notifier.Object.Erros.Should().HaveCount(2);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationStartDate().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationStartDate().Message);

        notifier.Object.Erros[1].Code.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationEndDate().Code);
        notifier.Object.Erros[1].Message.Should().Be(ErrorCodeConstant.ThereWillCurrentConfigurationEndDate().Message);
    }
}
