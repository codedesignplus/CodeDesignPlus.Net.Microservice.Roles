namespace CodeDesignPlus.Net.Microservice.Roles.Infrastructure.Repositories;

public class RoleRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RoleRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IRoleRepository
{
    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(RoleAggregate role, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(role, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
