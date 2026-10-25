namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.CreateRole;

public class CreateRoleCommandHandler(IRoleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateRoleCommand>
{
    public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<RoleAggregate>(request.Id, cancellationToken))
            return;

        var role = RoleAggregate.Create(request.Id, request.Name, request.Description, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(role, cancellationToken))
            return;

        await pubsub.PublishAsync(role.GetAndClearEvents(), cancellationToken);
    }
}