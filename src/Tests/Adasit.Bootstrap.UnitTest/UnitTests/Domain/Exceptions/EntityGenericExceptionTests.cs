namespace Adasit.Bootstrap.UnitTest.UnitTests.Domain.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using Adasit.Bootstrap.Domain.Conts;
using FluentAssertions;
using Xunit;
using DomainExceptions = Bootstrap.Domain.Exceptions;

public class EntityGenericExceptionTests
{
    [Fact(DisplayName = nameof(InstatiateWithOnlyMessage))]
    [Trait("Domain", "Exceptions - EntityGenericException")]
    public void InstatiateWithOnlyMessage()
    {
        //Arrange
        var msg = "New Exception";

        //Act
        var ex = new DomainExceptions.EntityGenericException(msg);

        //Assert
        ex.Message.Should().Be(msg);
        ex.Code.Should().Contain(ErrorsCodes.Generic);
    }

    [Fact(DisplayName = nameof(InstatiateWithMessageAndCode))]
    [Trait("Domain", "Exceptions - EntityGenericException")]
    public void InstatiateWithMessageAndCode()
    {
        //Arrange
        var msg = "New Exception";
        var ErrorCode = ErrorsCodes.Validation;

        //Act
        var ex = new DomainExceptions.EntityGenericException(msg, ErrorCode);

        //Assert
        ex.Message.Should().Be(msg);
        ex.Code.Should().Contain(ErrorCode);
    }

    [Fact(DisplayName = nameof(ThrowWithMessageAndCode))]
    [Trait("Domain", "Exceptions - EntityGenericException")]
    public void ThrowWithMessageAndCode()
    {
        //Arrange
        var msg = "New Exception";

        //Act
        Action action = () => throw new DomainExceptions.EntityGenericException(msg);

        //Assert
        action.Should().Throw<DomainExceptions.EntityGenericException>()
                .WithMessage(msg);
    }

    [Fact(DisplayName = nameof(ThrowWithMessageAndOnlyOneCode))]
    [Trait("Domain", "Exceptions - EntityGenericException")]
    public void ThrowWithMessageAndOnlyOneCode()
    {
        //Arrange
        var msg = "New Exception";
        var codes = new List<ErrorsCodes>() { ErrorsCodes.Validation, ErrorsCodes.Validation };

        //Act
        var act = new DomainExceptions.EntityGenericException(msg, codes);

        //Assert
        act.Code.Should().HaveCount(codes.Distinct().Count());
    }
}