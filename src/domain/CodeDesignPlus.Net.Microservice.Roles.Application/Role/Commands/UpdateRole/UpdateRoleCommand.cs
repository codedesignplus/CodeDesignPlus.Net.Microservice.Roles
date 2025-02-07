namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.UpdateRole;

[DtoGenerator]
public record UpdateRoleCommand(Guid Id, string Name, string Description, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateRoleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);

    }
}
