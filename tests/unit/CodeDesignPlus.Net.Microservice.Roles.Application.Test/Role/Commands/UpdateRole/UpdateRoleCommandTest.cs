using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.UpdateRole;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Commands.UpdateRole;

public class UpdateRoleCommandTest
{
    private readonly Validator validator;

    public UpdateRoleCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateRoleCommand(Guid.Empty, "RoleName", "RoleDescription", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Provided()
    {
        var command = new UpdateRoleCommand(Guid.NewGuid(), "RoleName", "RoleDescription", true);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
