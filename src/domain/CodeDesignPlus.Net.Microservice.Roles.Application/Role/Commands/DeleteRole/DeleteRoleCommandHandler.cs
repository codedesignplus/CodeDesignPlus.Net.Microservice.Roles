namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.DeleteRole;

public class DeleteRoleCommandHandler(IRoleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteRoleCommand>
{
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<RoleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.RoleNotFound);

        aggregate.Delete(user.IdUser);

        // TODO: Remove GUID Empty
        await repository.DeleteAsync<RoleAggregate>(aggregate.Id, Guid.Empty, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}