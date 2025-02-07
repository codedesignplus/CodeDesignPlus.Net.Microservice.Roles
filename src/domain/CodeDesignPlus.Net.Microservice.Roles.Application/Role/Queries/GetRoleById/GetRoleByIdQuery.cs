namespace CodeDesignPlus.Net.Microservice.Roles.Application.Role.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto>;

public class Validator : AbstractValidator<GetRoleByIdQuery>
{
    public Validator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
    }
}