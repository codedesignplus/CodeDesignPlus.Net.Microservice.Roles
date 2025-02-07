using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.DeleteRole;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Commands.DeleteRole;

public class DeleteRoleCommandTest
{
    private readonly Validator validator;

    public DeleteRoleCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteRoleCommand(Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteRoleCommand(Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
