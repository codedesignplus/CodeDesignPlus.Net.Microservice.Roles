using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Roles.Rest.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MediatR;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetAllRole;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetRoleById;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.CreateRole;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.UpdateRole;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.DeleteRole;
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Roles.Rest.Test.Controllers;

public class RoleControllerTest
{
    private readonly Mock<IMediator> mediatorMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly RoleController controller;

    public RoleControllerTest()
    {
        mediatorMock = new Mock<IMediator>();
        mapperMock = new Mock<IMapper>();
        controller = new RoleController(mediatorMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task GetRoles_ReturnsOkResult()
    {
        // Arrange
        var criteria = new C.Criteria();
        var roles = new List<RoleDto> {
            new() { Id = Guid.NewGuid(), Name = "Admin" }
        };

        var pagination = new Pagination<RoleDto>(roles, roles.Count, 1, 0);
        
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllRoleQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(pagination);

        // Act
        var result = await controller.GetRoles(criteria, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(pagination, okResult.Value);
    }

    [Fact]
    public async Task GetRoleById_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var role = new RoleDto { Id = roleId, Name = "Admin" };
        mediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(role);

        // Act
        var result = await controller.GetRoleById(roleId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(role, okResult.Value);
    }

    [Fact]
    public async Task CreateRole_ReturnsNoContentResult()
    {
        // Arrange
        var createRoleCommand = new CreateRoleCommand(Guid.NewGuid(), "Admin", "Administrator role");
        var createRoleDto = new CodeDesignPlus.Microservice.Api.Dtos.CreateRoleDto
        {
            Id = createRoleCommand.Id,
            Name = createRoleCommand.Name,
            Description = createRoleCommand.Description
        };

        mapperMock.Setup(m => m.Map<CreateRoleCommand>(createRoleDto)).Returns(createRoleCommand);
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateRoleCommand>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await controller.CreateRole(createRoleDto, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(It.IsAny<CreateRoleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRole_ReturnsNoContentResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var updateRoleCommand = new UpdateRoleCommand(Guid.NewGuid(), "Admin", "Administrator role", true);
        var updateRoleDto = new CodeDesignPlus.Microservice.Api.Dtos.UpdateRoleDto {
            Id = updateRoleCommand.Id,
            Name = updateRoleCommand.Name,
            Description = updateRoleCommand.Description,
            IsActive = updateRoleCommand.IsActive
        };
        mapperMock.Setup(m => m.Map<UpdateRoleCommand>(updateRoleDto)).Returns(updateRoleCommand);
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await controller.UpdateRole(roleId, updateRoleDto, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);

        mediatorMock.Verify(m => m.Send(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRole_ReturnsNoContentResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteRoleCommand>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await controller.DeleteRole(roleId, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
        mediatorMock.Verify(m => m.Send(It.IsAny<DeleteRoleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
