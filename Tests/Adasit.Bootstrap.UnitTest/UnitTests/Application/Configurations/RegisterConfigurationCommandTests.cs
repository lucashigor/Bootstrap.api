namespace Adasit.Bootstrap.UnitTest.UnitTests.Application.Configurations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.Domain.Repository;
using Adasit.Bootstrap.UnitTest.UnitTests.Domain.Configurations;
using FluentAssertions;
using Moq;
using Xunit;

[Collection(nameof(ConfigurationTestFixture))]
public class RegisterConfigurationCommandTests
{
    private readonly ConfigurationTestFixture fixture;
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IConfigurationRepository> configurationMock;
    private readonly Mock<IDateValidationHandler> dateValidationMock;
    private readonly Mock<Notifier> notifier;

    public RegisterConfigurationCommandTests(ConfigurationTestFixture fixture)
    {
        this.fixture = fixture;
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.configurationMock = new Mock<IConfigurationRepository>();
        this.notifier = new Mock<Notifier>();
        this.dateValidationMock = new Mock<IDateValidationHandler>();
    }

    [Fact(DisplayName = nameof(HandleRegisterConfigurationCommand_HappyPath_Async))]
    [Trait("Domain", "Configuration - RegisterConfigurationCommand")]
    public async Task HandleRegisterConfigurationCommand_HappyPath_Async()
    {
        var validData = fixture.GetValidConfiguration();

        var item = new RegisterConfigurationInput()
        {
            Description = validData.Description,
            FinalDate = validData.FinalDate,
            Name = validData.Name,
            StartDate = validData.StartDate,
            Value = validData.Value,
        };

        configurationMock.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Configuration>()
        {
            validData
        });

        var app = new RegisterConfigurationCommand(configurationMock.Object, unitOfWorkMock.Object, notifier.Object, dateValidationMock.Object);

        //Act
        var datetimeBefore = DateTime.UtcNow;

        var result = await app.Handle(item, CancellationToken.None);

        var datetimeAfter = DateTime.UtcNow.AddSeconds(1);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(), It.IsAny<CancellationToken>()), Times.Once());
        configurationMock.Verify(x => x.Insert(It.IsAny<Configuration>(), It.IsAny<CancellationToken>()), Times.Once());
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact(DisplayName = nameof(HandleRegisterConfigurationCommand_WarningInitalDateBeforeToday_Async))]
    [Trait("Domain", "Configuration - RegisterConfigurationCommand")]
    public async Task HandleRegisterConfigurationCommand_WarningInitalDateBeforeToday_Async()
    {
        var validData = fixture.GetValidConfiguration();

        var item = new RegisterConfigurationInput()
        {
            Description = validData.Description,
            FinalDate = validData.FinalDate,
            Name = validData.Name,
            StartDate = DateTimeOffset.UtcNow.AddDays(-2),
            Value = validData.Value,
        };

        configurationMock.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Configuration>()
        {
            validData
        });

        var app = new RegisterConfigurationCommand(configurationMock.Object, unitOfWorkMock.Object, notifier.Object, dateValidationMock.Object);

        //Act
        var datetimeBefore = DateTime.UtcNow;

        var result = await app.Handle(item, CancellationToken.None);

        var datetimeAfter = DateTime.UtcNow.AddSeconds(1);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        (result.StartDate > datetimeBefore).Should().BeTrue();

        notifier.Object.Erros.Should().BeEmpty();

        notifier.Object.Warnings.Should().HaveCount(1);
        notifier.Object.Warnings[0].Code.Should().Be(ErrorCodeConstant.StartDateCannotBeBeforeToToday().Code);
        notifier.Object.Warnings[0].Message.Should().Be(ErrorCodeConstant.StartDateCannotBeBeforeToToday().Message);

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(), It.IsAny<CancellationToken>()), Times.Once());
        configurationMock.Verify(x => x.Insert(It.IsAny<Configuration>(), It.IsAny<CancellationToken>()), Times.Once());
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact(DisplayName = nameof(HandleRegisterConfigurationCommand_ErrorFinalDateBeforeToday_Async))]
    [Trait("Domain", "Configuration - RegisterConfigurationCommand")]
    public async Task HandleRegisterConfigurationCommand_ErrorFinalDateBeforeToday_Async()
    {
        var validData = fixture.GetValidConfiguration();

        var item = new RegisterConfigurationInput()
        {
            Description = validData.Description,
            FinalDate = DateTimeOffset.UtcNow.AddDays(-2),
            Name = validData.Name,
            StartDate = DateTimeOffset.UtcNow.AddDays(-20),
            Value = validData.Value,
        };

        var app = new RegisterConfigurationCommand(configurationMock.Object, unitOfWorkMock.Object, notifier.Object, dateValidationMock.Object);

        //Act
        var result = await app.Handle(item, CancellationToken.None);

        //Assert
        result.Should().BeNull();

        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.EndDateCannotBeBeforeToToday().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.EndDateCannotBeBeforeToToday().Message);
    }

    [Fact(DisplayName = nameof(HandleRegisterConfigurationCommand_ErrorDomainValidation_Async))]
    [Trait("Domain", "Configuration - RegisterConfigurationCommand")]
    public async Task HandleRegisterConfigurationCommand_ErrorDomainValidation_Async()
    {
        var validData = fixture.GetValidConfiguration();

        var item = new RegisterConfigurationInput()
        {
            Description = validData.Description,
            FinalDate = DateTimeOffset.UtcNow.AddDays(-2),
            Name = fixture.GetStringRigthSize(150,1000),
            StartDate = DateTimeOffset.UtcNow.AddDays(-20),
            Value = validData.Value,
        };

        var app = new RegisterConfigurationCommand(configurationMock.Object, unitOfWorkMock.Object, notifier.Object, dateValidationMock.Object);

        //Act
        var result = await app.Handle(item, CancellationToken.None);

        //Assert
        result.Should().BeNull();

        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.Validation().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.Validation().Message);
        notifier.Object.Erros[0].InnerMessage.Should().Be(ErrorsMessages.BetweenLength.GetMessage(nameof(RegisterConfigurationInput.Name), 3, 100));
    }
}
