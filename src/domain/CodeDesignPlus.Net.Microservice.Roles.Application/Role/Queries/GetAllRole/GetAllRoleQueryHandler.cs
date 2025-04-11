using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetAllRole;

public class GetAllRoleQueryHandler(IRoleRepository repository, IMapper mapper) : IRequestHandler<GetAllRoleQuery, Pagination<RoleDto>>
{
    public async Task<Pagination<RoleDto>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var roles = await repository.MatchingAsync<RoleAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<RoleDto>>(roles);
    }
}
