using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetAllRole;

public record GetAllRoleQuery(C.Criteria Criteria) : IRequest<Pagination<RoleDto>>;

