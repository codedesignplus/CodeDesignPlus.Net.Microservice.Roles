using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.DeleteRole;
using CodeDesignPlus.Net.Microservice.Roles.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Commands.DeleteRole;

public class DeleteRoleCommandHandlerTest
{
    private readonly Mock<IRoleRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteRoleCommandHandler handler;

    public DeleteRoleCommandHandlerTest()
    {
        repositoryMock = new Mock<IRoleRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteRoleCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        DeleteRoleCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));
    
        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_RoleNotFound_ThrowsRoleNotFoundException()
    {
        // Arrange
        var request = new DeleteRoleCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.FindAsync<RoleAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((RoleAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));
    
        Assert.Equal(Errors.RoleNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.RoleNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesRoleAndPublishesEvents()
    {
        // Arrange
        var request = new DeleteRoleCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;
        var roleAggregate = RoleAggregate.Create(Guid.NewGuid(), "Admin", "Administrator role", Guid.NewGuid());

        repositoryMock
            .Setup(r => r.FindAsync<RoleAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(roleAggregate);

        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.DeleteAsync<RoleAggregate>(roleAggregate.Id, cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<RoleDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
