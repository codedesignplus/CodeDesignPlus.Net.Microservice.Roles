using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetRoleById;
using CodeDesignPlus.Net.Microservice.Roles.Domain;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Queries.GetRoleById;

public class GetRoleByIdQueryHandlerTest
{
    private readonly Mock<IRoleRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ICacheManager> cacheManagerMock;
    private readonly GetRoleByIdQueryHandler handler;

    public GetRoleByIdQueryHandlerTest()
    {
        repositoryMock = new Mock<IRoleRepository>();
        mapperMock = new Mock<IMapper>();
        cacheManagerMock = new Mock<ICacheManager>();
        handler = new GetRoleByIdQueryHandler(repositoryMock.Object, mapperMock.Object, cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        GetRoleByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));
        
        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_RoleExistsInCache_ReturnsRoleFromCache()
    {
        // Arrange
        var request = new GetRoleByIdQuery(Guid.NewGuid());
        var roleDto = new RoleDto()
        {
            Id = request.Id,
            Name = "Role",
            Description = "Role Description"
        };
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        cacheManagerMock.Setup(x => x.GetAsync<RoleDto>(request.Id.ToString())).ReturnsAsync(roleDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(roleDto, result);
        repositoryMock.Verify(x => x.FindAsync<RoleAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_RoleNotInCache_RoleExistsInRepository_ReturnsRole()
    {
        // Arrange
        var request = new GetRoleByIdQuery(Guid.NewGuid());
        var role = RoleAggregate.Create(request.Id, "Role", "Role Description", Guid.NewGuid());
        var roleDto = new RoleDto()
        {
            Id = request.Id,
            Name = "Role",
            Description = "Role Description"
        };
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<RoleAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(role);
        mapperMock.Setup(x => x.Map<RoleDto>(role)).Returns(roleDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(roleDto, result);
        cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), roleDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RoleNotInCache_RoleNotInRepository_ThrowsRoleNotFoundException()
    {
        // Arrange
        var request = new GetRoleByIdQuery(Guid.NewGuid());
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<RoleAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((RoleAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.RoleNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.RoleNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
