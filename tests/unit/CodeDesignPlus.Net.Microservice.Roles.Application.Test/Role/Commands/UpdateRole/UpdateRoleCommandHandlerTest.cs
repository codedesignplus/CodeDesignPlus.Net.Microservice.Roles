using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.UpdateRole;
using CodeDesignPlus.Net.Microservice.Roles.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Test.Role.Commands.UpdateRole;

public class UpdateRoleCommandHandlerTest
{
    private readonly Mock<IRoleRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateRoleCommandHandler handler;

    public UpdateRoleCommandHandlerTest()
    {
        repositoryMock = new Mock<IRoleRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateRoleCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        UpdateRoleCommand request = null!;
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
        var request = new UpdateRoleCommand(Guid.NewGuid(), "New Role Name", "New Role Description", true);
        repositoryMock.Setup(r => r.FindAsync<RoleAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((RoleAggregate)null!);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));
    
        Assert.Equal(Errors.RoleNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.RoleNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesRoleAndPublishesEvents()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var request = new UpdateRoleCommand(Guid.NewGuid(), "New Role Name", "New Role Description", true);
        var role = RoleAggregate.Create(request.Id, "Role Name", "Role Description", Guid.NewGuid());

        repositoryMock.Setup(r => r.FindAsync<RoleAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(role);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());
        

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(role, cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<RoleUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
