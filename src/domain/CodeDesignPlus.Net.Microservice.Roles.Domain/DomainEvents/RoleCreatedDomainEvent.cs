namespace CodeDesignPlus.Net.Microservice.Roles.Domain.DomainEvents;

[EventKey<RoleAggregate>(1, "RoleCreatedDomainEvent")]
public class RoleCreatedDomainEvent(
    Guid aggregateId,
    string name,
    string description,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{

    public string Name { get; private set; } = name;

    public string Description { get; private set; } = description;

    public bool IsActive { get; private set; } = isActive;

    public static RoleCreatedDomainEvent Create(Guid aggregateId, string name, string description, bool isActive)
    {
        return new RoleCreatedDomainEvent(aggregateId, name, description, isActive);
    }
}
