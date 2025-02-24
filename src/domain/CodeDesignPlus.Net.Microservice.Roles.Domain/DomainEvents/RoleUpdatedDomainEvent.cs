namespace CodeDesignPlus.Net.Microservice.Roles.Domain.DomainEvents;

[EventKey<RoleAggregate>(1, "RoleUpdatedDomainEvent")]
public class RoleUpdatedDomainEvent(
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

    public static RoleUpdatedDomainEvent Create(Guid aggregateId, string name, string description, bool isActive)
    {
        return new RoleUpdatedDomainEvent(aggregateId, name, description, isActive);
    }
}
