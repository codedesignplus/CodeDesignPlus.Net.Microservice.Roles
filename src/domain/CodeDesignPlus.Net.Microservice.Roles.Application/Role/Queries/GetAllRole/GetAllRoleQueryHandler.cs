namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetAllRole;

public class GetAllRoleQueryHandler(IRoleRepository repository, IMapper mapper) : IRequestHandler<GetAllRoleQuery, List<RoleDto>>
{
    public async Task<List<RoleDto>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var roles = await repository.MatchingAsync<RoleAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<RoleDto>>(roles);
    }
}
