namespace Adasit.Bootstrap.UnitTest.UnitTests.Application.Configurations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models.Response;
using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using Adasit.Bootstrap.Domain.Conts;
using Adasit.Bootstrap.Domain.Entity;
using Adasit.Bootstrap.Domain.Repository;
using Adasit.Bootstrap.UnitTest.UnitTests.Domain.Configurations;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

[Collection(nameof(ConfigurationTestFixture))]
public class ChangeConfigurationCommandTests
{
    private readonly ConfigurationTestFixture fixture;
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IConfigurationRepository> configurationMock;
    private readonly Mock<IDateValidationHandler> dateValidationMock;
    private readonly Mock<IMediator> mediator;
    private readonly Mock<Notifier> notifier;

    public ChangeConfigurationCommandTests(ConfigurationTestFixture fixture)
    {
        this.fixture = fixture;
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.configurationMock = new Mock<IConfigurationRepository>();
        this.notifier = new Mock<Notifier>();
        this.dateValidationMock = new Mock<IDateValidationHandler>();
        this.mediator = new Mock<IMediator>();
    }

    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_HappyPath_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_HappyPath_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Id = Guid.NewGuid();
        entity.Name = fixture.GetStringRigthSize(5, 100);
        entity.Value = fixture.GetStringRigthSize(5, 100);
        entity.Description = fixture.GetStringRigthSize(5, 1000);
        entity.StartDate = DateTimeOffset.UtcNow.AddDays(10);
        entity.FinalDate = DateTimeOffset.UtcNow.AddDays(30);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        configurationMock.Setup(x => x.GetById(It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.GetValidConfiguration());

        //Act
        await app.Handle(entity, CancellationToken.None);

        //Assert
        notifier.Object.Erros.Should().BeEmpty();
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        configurationMock.Verify(x => x.Update(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_NotFound_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_NotFound_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        //Act
        var ret = await app.Handle(entity, CancellationToken.None);

        //Assert
        ret.Should().BeNull();

        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.ConfigurationNotFound().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.ConfigurationNotFound().Message);
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        configurationMock.Verify(x => x.Update(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());
    }

    //Found - Set to invalid domain
    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_SetToInvalidDomain_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_SetToInvalidDomain_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Id = Guid.NewGuid();
        entity.Name = fixture.GetStringRigthSize(1, 2);
        entity.Value = fixture.GetStringRigthSize(5, 100);
        entity.Description = fixture.GetStringRigthSize(5, 1000);
        entity.StartDate = DateTimeOffset.UtcNow.AddDays(10);
        entity.FinalDate = DateTimeOffset.UtcNow.AddDays(30);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        configurationMock.Setup(x => x.GetById(It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.GetValidConfiguration());

        //Act
        var ret = await app.Handle(entity, CancellationToken.None);

        //Assert
        ret.Should().BeNull();

        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.Validation().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.Validation().Message);
        notifier.Object.Erros[0].InnerMessage.Should().Be(ErrorsMessages.BetweenLength.GetMessage(nameof(Configuration.Name), 3, 100));
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        configurationMock.Verify(x => x.Update(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());
    }

    //Error - Found Closed (as closed final date has been passed)- Allow to Update only description
    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_SetToInvalidDomain_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_FoundClosedNotAllowToUpdateNameValueAndDates_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Name = fixture.GetStringRigthSize(3, 100);
        entity.Value = fixture.GetStringRigthSize(5, 100);
        entity.Description = fixture.GetStringRigthSize(5, 1000);
        entity.StartDate = DateTimeOffset.UtcNow.AddDays(10);
        entity.FinalDate = DateTimeOffset.UtcNow.AddDays(30);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        var inDatabase = new Configuration(
            fixture.GetStringRigthSize(5, 100),
            fixture.GetStringRigthSize(5, 300),
            fixture.GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddMonths(-2),
            DateTimeOffset.UtcNow.AddMonths(-1));

        entity.Id = inDatabase.Id;

        configurationMock.Setup(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(inDatabase);

        //Act
        var ret = await app.Handle(entity, CancellationToken.None);

        //Assert
        ret.Should().BeNull();

        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.OnlyDescriptionAreAvaliableToChangedOnClosedConfiguration().Message);
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        configurationMock.Verify(x => x.Update(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());
    }

    //Success - Found Closed (as closed final date has been passed)- Allow to Update only description
    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_SetToInvalidDomain_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_FoundClosedAllowToUpdateDescription_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Description = fixture.GetStringRigthSize(5, 1000);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        var inDatabase = new Configuration(
            fixture.GetStringRigthSize(5, 100),
            fixture.GetStringRigthSize(5, 300),
            fixture.GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddMonths(-2),
            DateTimeOffset.UtcNow.AddMonths(-1));

        entity.Id = inDatabase.Id;
        entity.Name = inDatabase.Name;
        entity.Value = inDatabase.Value;
        entity.StartDate = inDatabase.StartDate;
        entity.FinalDate = inDatabase.FinalDate;

        configurationMock.Setup(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(inDatabase);

        //Act
        var datetimeBefore = DateTimeOffset.UtcNow;
        Thread.Sleep(1);

        var ret = await app.Handle(entity, CancellationToken.None);

        //Assert
        ret.Should().NotBeNull();

        notifier.Object.Erros.Should().BeEmpty();
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        configurationMock.Verify(x => x.Update(It.Is<Configuration>(x => x.LastUpdateAt > datetimeBefore
            && x.Description.Equals(entity.Description)
            && x.StartDate.Equals(inDatabase.StartDate)),
            It.IsAny<CancellationToken>()),
            Times.Once());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());
    }

    //Found In Course - When Updates description, the descripition and lastUpdate receives the update
    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_FoundInCourseAllowToUpdateDescription_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_FoundInCourseAllowToUpdateDescription_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Description = fixture.GetStringRigthSize(5, 1000);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        var inDatabase = new Configuration(
            fixture.GetStringRigthSize(5, 100),
            fixture.GetStringRigthSize(5, 300),
            fixture.GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddMonths(-2),
            DateTimeOffset.UtcNow.AddMonths(1));

        entity.Id = inDatabase.Id;
        entity.Name = inDatabase.Name;
        entity.Value = inDatabase.Value;
        entity.StartDate = inDatabase.StartDate;
        entity.FinalDate = inDatabase.FinalDate;

        configurationMock.Setup(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(inDatabase);

        //Act
        var datetimeBefore = DateTimeOffset.UtcNow;
        Thread.Sleep(1);

        var ret = await app.Handle(entity, CancellationToken.None);

        //Assert
        ret.Should().NotBeNull();

        ret.Id.Should().Be(inDatabase.Id);

        notifier.Object.Erros.Should().BeEmpty();
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        configurationMock.Verify(x => x.Update(It.Is<Configuration>(x => x.LastUpdateAt > datetimeBefore
            && x.Description.Equals(entity.Description)
            && x.StartDate.Equals(inDatabase.StartDate)),
            It.IsAny<CancellationToken>()),
            Times.Once());

        configurationMock.Verify(x => x.Insert(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());
    }

    //Found In Course - When Updates description, the descripition and lastUpdate receives the update
    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_FoundInCourseCreateAnotherConfigurationWithNewValues_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_FoundInCourseCreateAnotherConfigurationWithNewValues_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Description = fixture.GetStringRigthSize(5, 1000);
        entity.Value = fixture.GetStringRigthSize(3, 100);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        var inDatabase = new Configuration(
            fixture.GetStringRigthSize(5, 100),
            fixture.GetStringRigthSize(5, 300),
            fixture.GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddMonths(-2),
            DateTimeOffset.UtcNow.AddMonths(1));

        entity.Id = inDatabase.Id;
        entity.Name = inDatabase.Name;
        entity.StartDate = inDatabase.StartDate;
        entity.FinalDate = inDatabase.FinalDate;

        configurationMock.Setup(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(inDatabase);

        mediator.Setup(x => x.Send(It.IsAny<RegisterConfigurationInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfigurationOutputDto(Guid.NewGuid(),
                entity.Name,
                entity.Value,
                entity.Description,
                entity.StartDate,
                entity.FinalDate
                ));

        //Act
        var datetimeBefore = DateTimeOffset.UtcNow;
        Thread.Sleep(1);

        var ret = await app.Handle(entity, CancellationToken.None);

        Thread.Sleep(1);
        var datetimeAfter = DateTimeOffset.UtcNow;

        //Assert
        ret.Should().NotBeNull();

        ret.Id.Should().NotBe(inDatabase.Id);

        notifier.Object.Erros.Should().BeEmpty();
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        configurationMock.Verify(x => x.Update(It.Is<Configuration>(x => x.LastUpdateAt > datetimeBefore
            && x.Description.Equals(inDatabase.Description)
            && x.StartDate.Equals(inDatabase.StartDate)
            && x.Name.Equals(inDatabase.Name)
            && x.FinalDate < datetimeAfter
            && x.FinalDate > datetimeBefore),
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Send(It.Is<RegisterConfigurationInput>(x =>
             x.Description.Equals(entity.Description)
            && x.StartDate > datetimeBefore
            && x.Name.Equals(entity.Name)
            && x.Value.Equals(entity.Value)
            && x.FinalDate.Equals(entity.FinalDate)),
            It.IsAny<CancellationToken>()),
            Times.Once());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());
    }

    //Error - Found In Course - When Updates Final Date. Is Not allowes to update before today (Warning set to UtcNow) update field LastUpdate
    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_FoundInCourseChangeFinalDateToBeforeToday_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_FoundInCourseChangeFinalDateToBeforeToday_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Description = fixture.GetStringRigthSize(5, 1000);
        entity.Name = fixture.GetStringRigthSize(3, 100);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        var inDatabase = new Configuration(
            fixture.GetStringRigthSize(5, 100),
            fixture.GetStringRigthSize(5, 300),
            fixture.GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddMonths(-2),
            DateTimeOffset.UtcNow.AddMonths(1));

        entity.Id = inDatabase.Id;
        entity.Name = inDatabase.Name;
        entity.Value = inDatabase.Value;
        entity.StartDate = inDatabase.StartDate;
        entity.FinalDate = DateTimeOffset.UtcNow.AddDays(-1);

        configurationMock.Setup(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(inDatabase);

        mediator.Setup(x => x.Send(It.IsAny<RegisterConfigurationInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfigurationOutputDto(Guid.NewGuid(),
                entity.Name,
                entity.Value,
                entity.Description,
                entity.StartDate,
                entity.FinalDate
                ));

        //Act
        var datetimeBefore = DateTimeOffset.UtcNow;
        Thread.Sleep(1);

        var ret = await app.Handle(entity, CancellationToken.None);

        Thread.Sleep(1);
        var datetimeAfter = DateTimeOffset.UtcNow;

        //Assert
        ret.Should().NotBeNull();

        ret.Id.Should().Be(inDatabase.Id);

        notifier.Object.Erros.Should().BeEmpty();
        notifier.Object.Warnings.Should().HaveCount(1);

        notifier.Object.Warnings[0].Code.Should().Be(ErrorCodeConstant.ItsNotAllowedToChangeFinalDatetoBeforeToday().Code);
        notifier.Object.Warnings[0].Message.Should().Be(ErrorCodeConstant.ItsNotAllowedToChangeFinalDatetoBeforeToday().Message);

        configurationMock.Verify(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        configurationMock.Verify(x => x.Update(It.Is<Configuration>(x => x.LastUpdateAt > datetimeBefore
            && x.Description.Equals(inDatabase.Description)
            && x.StartDate.Equals(inDatabase.StartDate)
            && x.Name.Equals(inDatabase.Name)
            && x.FinalDate < datetimeAfter
            && x.FinalDate > datetimeBefore),
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Send(It.IsAny<RegisterConfigurationInput>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());
    }

    //Success - Found In Course - When Updates Final Date. Is Not allowes to update before today (Warning set to UtcNow) update field LastUpdate
    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_FoundInCourseChangeFinalDate_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_FoundInCourseChangeFinalDate_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Description = fixture.GetStringRigthSize(5, 1000);
        entity.Name = fixture.GetStringRigthSize(3, 100);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        var inDatabase = new Configuration(
            fixture.GetStringRigthSize(5, 100),
            fixture.GetStringRigthSize(5, 300),
            fixture.GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddMonths(-2),
            DateTimeOffset.UtcNow.AddMonths(1));

        entity.Id = inDatabase.Id;
        entity.Name = inDatabase.Name;
        entity.Value = inDatabase.Value;
        entity.StartDate = inDatabase.StartDate;
        entity.FinalDate = DateTimeOffset.UtcNow.AddDays(1);

        configurationMock.Setup(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(inDatabase);

        mediator.Setup(x => x.Send(It.IsAny<RegisterConfigurationInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfigurationOutputDto(Guid.NewGuid(),
                entity.Name,
                entity.Value,
                entity.Description,
                entity.StartDate,
                entity.FinalDate
                ));

        //Act
        var datetimeBefore = DateTimeOffset.UtcNow;
        Thread.Sleep(1);

        var ret = await app.Handle(entity, CancellationToken.None);

        Thread.Sleep(1);
        var datetimeAfter = DateTimeOffset.UtcNow;

        //Assert
        ret.Should().NotBeNull();

        ret.Id.Should().Be(inDatabase.Id);

        notifier.Object.Erros.Should().BeEmpty();
        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());

        configurationMock.Verify(x => x.Update(It.Is<Configuration>(x => x.LastUpdateAt > datetimeBefore
            && x.Description.Equals(inDatabase.Description)
            && x.StartDate.Equals(inDatabase.StartDate)
            && x.Name.Equals(inDatabase.Name)
            && x.FinalDate.Equals(entity.FinalDate)),
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Send(It.IsAny<RegisterConfigurationInput>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [Fact(DisplayName = nameof(HandleChangeConfigurationCommand_FoundInCourseChangeInitalDateOnConfigInCourse_Async))]
    [Trait("Domain", "Configuration - ChangeConfigurationCommand")]
    public async Task HandleChangeConfigurationCommand_FoundInCourseChangeInitalDateOnConfigInCourse_Async()
    {
        //Arrange
        var entity = new ModifyConfigurationInput();
        entity.Description = fixture.GetStringRigthSize(5, 1000);
        entity.Name = fixture.GetStringRigthSize(3, 100);

        var app = new ChangeConfigurationCommand(configurationMock.Object,
            unitOfWorkMock.Object,
            dateValidationMock.Object,
            mediator.Object,
            notifier.Object);

        var inDatabase = new Configuration(
            fixture.GetStringRigthSize(5, 100),
            fixture.GetStringRigthSize(5, 300),
            fixture.GetStringRigthSize(5, 1000),
            DateTimeOffset.UtcNow.AddMonths(-2),
            DateTimeOffset.UtcNow.AddMonths(1));

        entity.Id = inDatabase.Id;
        entity.Name = inDatabase.Name;
        entity.Value = inDatabase.Value;
        entity.StartDate = DateTimeOffset.UtcNow.AddMonths(-1);
        entity.FinalDate = inDatabase.FinalDate;

        configurationMock.Setup(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(inDatabase);

        mediator.Setup(x => x.Send(It.IsAny<RegisterConfigurationInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfigurationOutputDto(Guid.NewGuid(),
                entity.Name,
                entity.Value,
                entity.Description,
                entity.StartDate,
                entity.FinalDate
                ));

        //Act
        var datetimeBefore = DateTimeOffset.UtcNow;
        Thread.Sleep(1);

        var ret = await app.Handle(entity, CancellationToken.None);

        Thread.Sleep(1);
        var datetimeAfter = DateTimeOffset.UtcNow;

        //Assert
        ret.Should().BeNull();

        notifier.Object.Erros.Should().HaveCount(1);
        notifier.Object.Erros[0].Code.Should().Be(ErrorCodeConstant.ItsNotAllowedToChangeInitialDate().Code);
        notifier.Object.Erros[0].Message.Should().Be(ErrorCodeConstant.ItsNotAllowedToChangeInitialDate().Message);

        notifier.Object.Warnings.Should().BeEmpty();

        configurationMock.Verify(x => x.GetById(entity.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

        mediator.Verify(x => x.Publish(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        configurationMock.Verify(x => x.Update(It.Is<Configuration>(x => x.LastUpdateAt > datetimeBefore
            && x.Description.Equals(inDatabase.Description)
            && x.StartDate.Equals(inDatabase.StartDate)
            && x.Name.Equals(inDatabase.Name)
            && x.FinalDate < datetimeAfter
            && x.FinalDate > datetimeBefore),
            It.IsAny<CancellationToken>()),
            Times.Never());

        mediator.Verify(x => x.Send(It.IsAny<RegisterConfigurationInput>(),
            It.IsAny<CancellationToken>()),
            Times.Never());

        dateValidationMock.Verify(x => x.Handle(It.IsAny<Configuration>(),
            It.IsAny<CancellationToken>()),
            Times.Never());
    }
}
