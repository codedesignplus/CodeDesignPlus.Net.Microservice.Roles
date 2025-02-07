using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.CreateRole;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Commands.CreateRole;

public class CreateRoleCommandTest
{
    private readonly Validator validator;

    public CreateRoleCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateRoleCommand(Guid.Empty, "RoleName", "RoleDescription");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new CreateRoleCommand(Guid.NewGuid(), "RoleName", "RoleDescription");
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
