namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.DeleteRole;

[DtoGenerator]
public record DeleteRoleCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteRoleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
