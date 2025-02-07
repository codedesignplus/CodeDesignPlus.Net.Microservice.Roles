using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.CreateRole;
using CodeDesignPlus.Net.Microservice.Roles.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Commands.CreateRole;

public class CreateRoleCommandHandlerTest
{
    private readonly Mock<IRoleRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly CreateRoleCommandHandler handler;

    public CreateRoleCommandHandlerTest()
    {
        repositoryMock = new Mock<IRoleRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateRoleCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        CreateRoleCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_RoleAlreadyExists_ThrowsRoleAlreadyExistsException()
    {
        // Arrange
        var request = new CreateRoleCommand(Guid.NewGuid(), "Admin", "Administrator role");
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(repo => repo.ExistsAsync<RoleAggregate>(request.Id, cancellationToken)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.RoleAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.RoleAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesRoleAndPublishesEvents()
    {
        // Arrange
        var request = new CreateRoleCommand(Guid.NewGuid(), "Admin", "Administrator role");
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(repo => repo.ExistsAsync<RoleAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
        userContextMock.Setup(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<RoleAggregate>(), cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<RoleCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
