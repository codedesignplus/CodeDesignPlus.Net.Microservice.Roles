namespace CodeDesignPlus.Net.Microservice.Roles.Infrastructure.Repositories;

public class RoleRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RoleRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IRoleRepository
{
   
}