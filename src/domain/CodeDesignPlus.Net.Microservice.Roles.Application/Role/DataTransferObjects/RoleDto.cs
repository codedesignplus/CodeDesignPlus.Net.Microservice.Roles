namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.DataTransferObjects;

public class RoleDto: IDtoBase
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}