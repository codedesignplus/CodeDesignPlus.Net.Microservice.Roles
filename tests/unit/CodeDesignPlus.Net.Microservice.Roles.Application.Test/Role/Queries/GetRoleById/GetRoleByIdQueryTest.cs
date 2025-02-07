
using System;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetRoleById;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Queries.GetRoleById;

public class GetRoleByIdQueryTest
{
    private readonly Validator validator;

    public GetRoleByIdQueryTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetRoleByIdQuery(Guid.Empty);
        var result = validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Not_Empty()
    {
        var query = new GetRoleByIdQuery(Guid.NewGuid());
        var result = validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
