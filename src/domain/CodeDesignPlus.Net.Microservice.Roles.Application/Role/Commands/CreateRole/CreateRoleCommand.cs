namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.CreateRole;

[DtoGenerator]
public record CreateRoleCommand(Guid Id, string Name, string Description) : IRequest;

public class Validator : AbstractValidator<CreateRoleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
    }
}
