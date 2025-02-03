namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.CreateRole;

public class CreateRoleCommandHandler(IRoleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateRoleCommand>
{
    public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<RoleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.RoleAlreadyExists);

        var role = RoleAggregate.Create(request.Id, request.Name, request.Description, user.IdUser);

        await repository.CreateAsync(role, cancellationToken);

        await pubsub.PublishAsync(role.GetAndClearEvents(), cancellationToken);
    }
}