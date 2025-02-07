namespace CodeDesignPlus.Net.Microservice.Roles.Domain.Test;

public class RoleAggregateTest
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateRoleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Admin";
        var description = "Administrator role";
        var createdBy = Guid.NewGuid();

        // Act
        var role = RoleAggregate.Create(id, name, description, createdBy);
        var domainEvents = role.GetAndClearEvents();
        var domainEvent = domainEvents[0] as RoleCreatedDomainEvent;


        // Assert
        Assert.NotNull(role);
        Assert.Equal(id, role.Id);
        Assert.Equal(name, role.Name);
        Assert.Equal(description, role.Description);
        Assert.True(role.IsActive);
        Assert.Equal(createdBy, role.CreatedBy);


        Assert.NotNull(domainEvent);
        Assert.Equal(id, domainEvent.AggregateId);
        Assert.Equal(name, domainEvent.Name);
        Assert.Equal(description, domainEvent.Description);
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateRoleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Admin";
        var description = "Administrator role";
        var createdBy = Guid.NewGuid();
        var role = RoleAggregate.Create(id, name, description, createdBy);

        var newName = "Super Admin";
        var newDescription = "Super Administrator role";
        var updatedBy = Guid.NewGuid();

        // Act
        role.Update(newName, newDescription, false, updatedBy);
        var domainEvents = role.GetAndClearEvents();
        var domainEvent = domainEvents.FirstOrDefault(x => x is RoleUpdatedDomainEvent) as RoleUpdatedDomainEvent;

        // Assert
        Assert.Equal(newName, role.Name);
        Assert.Equal(newDescription, role.Description);
        Assert.False(role.IsActive);
        Assert.Equal(updatedBy, role.UpdatedBy);

        Assert.NotNull(domainEvent);
        Assert.Equal(id, domainEvent.AggregateId);
        Assert.Equal(newName, domainEvent.Name);
        Assert.Equal(newDescription, domainEvent.Description);
    }

    [Fact]
    public void Delete_ValidParameters_ShouldDeactivateRoleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Admin";
        var description = "Administrator role";
        var createdBy = Guid.NewGuid();
        var role = RoleAggregate.Create(id, name, description, createdBy);

        var deletedBy = Guid.NewGuid();

        // Act
        role.Delete(deletedBy);
        var domainEvents = role.GetAndClearEvents();
        var domainEvent = domainEvents.FirstOrDefault(x => x is RoleDeletedDomainEvent) as RoleDeletedDomainEvent;

        // Assert
        Assert.False(role.IsActive);
        Assert.Equal(deletedBy, role.UpdatedBy);

        Assert.NotNull(domainEvent);
        Assert.Equal(id, domainEvent.AggregateId);
    }
}
