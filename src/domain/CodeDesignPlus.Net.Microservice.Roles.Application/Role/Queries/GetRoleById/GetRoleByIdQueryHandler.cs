namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetRoleById;

public class GetRoleByIdQueryHandler(IRoleRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<RoleDto>(request.Id.ToString());

        var role = await repository.FindAsync<RoleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(role, Errors.RoleNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<RoleDto>(role));

        return mapper.Map<RoleDto>(role);
    }
}
