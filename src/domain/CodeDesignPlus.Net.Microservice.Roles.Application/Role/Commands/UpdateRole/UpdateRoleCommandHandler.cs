namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.UpdateRole;

public class UpdateRoleCommandHandler(IRoleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateRoleCommand>
{
    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var role = await repository.FindAsync<RoleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(role, Errors.RoleNotFound);

        role.Update(request.Name, request.Description, request.IsActive, user.IdUser);

        await repository.UpdateAsync(role, cancellationToken);

        await pubsub.PublishAsync(role.GetAndClearEvents(), cancellationToken);
    }
}