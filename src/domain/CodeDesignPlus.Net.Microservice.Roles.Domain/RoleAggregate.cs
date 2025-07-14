namespace CodeDesignPlus.Net.Microservice.Roles.Domain;

public class RoleAggregate(Guid id) : AggregateRootBase(id)
{
    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    private RoleAggregate(Guid id, string name, string description, Guid createdBy) : this(id)
    {
        this.Name = name;
        this.Description = description;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(RoleCreatedDomainEvent.Create(Id, Name, Description, IsActive));
    }

    public static RoleAggregate Create(Guid id, string name, string description, Guid createBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.RoleIdIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.RoleNameIsInvalid);
        DomainGuard.IsNullOrEmpty(description, Errors.RoleDescriptionIsInvalid);
        DomainGuard.GuidIsEmpty(createBy, Errors.CreatedByIsInvalid);

        return new RoleAggregate(id, name, description, createBy);
    }

    public void Update(string name, string description, bool IsActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.RoleNameIsInvalid);
        DomainGuard.IsNullOrEmpty(description, Errors.RoleDescriptionIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.CreatedByIsInvalid);

        this.Name = name;
        this.Description = description;
        this.IsActive = IsActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(RoleUpdatedDomainEvent.Create(Id, Name, Description, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.CreatedByIsInvalid);

        this.IsDeleted = true;
        this.IsActive = false;
        this.DeletedAt = SystemClock.Instance.GetCurrentInstant();
        this.DeletedBy = deletedBy;

        AddEvent(RoleDeletedDomainEvent.Create(Id, Name, Description, IsActive));
    }
}
