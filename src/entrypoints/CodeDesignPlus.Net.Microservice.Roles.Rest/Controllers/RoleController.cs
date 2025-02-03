namespace CodeDesignPlus.Net.Microservice.Roles.Rest.Controllers;

/// <summary>
/// Controller for managing the Roles.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class RoleController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Roles.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Roles.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Roles.</returns>
    [HttpGet]
    public async Task<IActionResult> GetRoles([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllRoleQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Role by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Role.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Role.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Role.
    /// </summary>
    /// <param name="data">Data for creating the Role.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateRoleCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Role.
    /// </summary>
    /// <param name="id">The unique identifier of the Role.</param>
    /// <param name="data">Data for updating the Role.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<UpdateRoleCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Role.
    /// </summary>
    /// <param name="id">The unique identifier of the Role.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteRoleCommand(id), cancellationToken);

        return NoContent();
    }
}