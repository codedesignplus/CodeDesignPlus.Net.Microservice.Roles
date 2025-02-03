namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto>;

