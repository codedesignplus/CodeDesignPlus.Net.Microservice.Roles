
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetAllRole;
using CodeDesignPlus.Net.Microservice.Roles.Domain;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Queries.GetAllRole;

public class GetAllRoleQueryHandlerTest
{
    private readonly Mock<IRoleRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly GetAllRoleQueryHandler handler;

    public GetAllRoleQueryHandlerTest()
    {
        repositoryMock = new Mock<IRoleRepository>();
        mapperMock = new Mock<IMapper>();
        handler = new GetAllRoleQueryHandler(repositoryMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        GetAllRoleQuery request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsRoleDtos()
    {
        // Arrange
        var request = new GetAllRoleQuery(null!);
        var cancellationToken = CancellationToken.None;
        var roleAggregate = RoleAggregate.Create(Guid.NewGuid(), "Role", "Description", Guid.NewGuid());
        var dto = new RoleDto()
        {
            Id = roleAggregate.Id,
            Name = roleAggregate.Name,
            Description = roleAggregate.Description
        };
        var roles = new List<RoleAggregate> { roleAggregate };
        var roleDtos = new List<RoleDto> { dto };

        var pagination = Pagination<RoleAggregate>.Create(roles, roles.Count(), 10, 0);

        repositoryMock
            .Setup(repo => repo.MatchingAsync<RoleAggregate>(request.Criteria, cancellationToken))
            .ReturnsAsync(pagination);
        mapperMock
            .Setup(mapper => mapper.Map<Pagination<RoleDto>>(pagination))
            .Returns(Pagination<RoleDto>.Create(roleDtos, roleDtos.Count, 10, 0));

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.Equal(roleDtos, result.Data);
    }
}
