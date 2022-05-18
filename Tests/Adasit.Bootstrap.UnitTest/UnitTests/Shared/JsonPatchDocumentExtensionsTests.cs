namespace Adasit.Bootstrap.UnitTest.UnitTests.Shared;
using System;
using System.Collections.Generic;
using Adasit.Bootstrap.Application;
using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Domain.Entity;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Xunit;

public class JsonPatchDocumentExtensionsTests
{
    [Fact(DisplayName = nameof(InvalidOperationJsonPatchDocument))]
    [Trait("Domain", "Shared - JsonPatchDocument")]
    public void InvalidOperationJsonPatchDocument()
    {
        //Arrange
        var command = new JsonPatchDocument<Configuration>();

        command.Remove(x => x.Value);

        //Act
        Action act = () => command.Validate(
                OperationType.Replace,
                new List<string> { nameof(Configuration.Name),
                    nameof(Configuration.Value),
                    nameof(Configuration.Description),
                    nameof(Configuration.StartDate),
                    nameof(Configuration.FinalDate)}
                );

        //Assert
        act.Should().Throw<BusinessException>().WithMessage(ErrorCodeConstant.InvalidOperationOnPatch().Message);
    }

    [Fact(DisplayName = nameof(InvalidPathJsonPatchDocument))]
    [Trait("Domain", "Shared - JsonPatchDocument")]
    public void InvalidPathJsonPatchDocument()
    {
        //Arrange
        var command = new JsonPatchDocument<Configuration>();

        command.Replace(x => x.Value, "asd");
        command.Replace(x => x.Description, "asdasd");

        //Act
        Action act = () => command.Validate(
                OperationType.Replace,
                new List<string> { nameof(Configuration.Name) }
                );

        //Assert
        act.Should().Throw<BusinessException>().WithMessage(ErrorCodeConstant.InvalidPathOnPatch().Message);
    }

    [Fact(DisplayName = nameof(SuccessPathJsonPatchDocument))]
    [Trait("Domain", "Shared - JsonPatchDocument")]
    public void SuccessPathJsonPatchDocument()
    {
        //Arrange
        var command = new JsonPatchDocument<Configuration>();

        command.Replace(x => x.Value, "asd");
        
        //Act
        Action act = () => command.Validate(
                OperationType.Replace,
                new List<string> { $"/{nameof(Configuration.Value)}" }
                );

        //Assert
        act.Should().NotThrow();
    }
}
