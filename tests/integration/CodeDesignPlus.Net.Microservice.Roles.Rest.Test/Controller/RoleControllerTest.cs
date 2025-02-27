namespace CodeDesignPlus.Net.Microservice.Roles.Rest.Test.Controller;

public class RoleControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    public RoleControllerTest(Server<Program> server) : base(server)
    {        
        server.InMemoryCollection = (x) =>
        {
            x.Add("Vault:Enable", "false");
            x.Add("Vault:Address", "http://localhost:8200");
            x.Add("Vault:Token", "root");
            x.Add("Solution", "CodeDesignPlus");
            x.Add("AppName", "my-test");
            x.Add("RabbitMQ:UserName", "guest");
            x.Add("RabbitMQ:Password", "guest");
            x.Add("Security:ValidAudiences:0", Guid.NewGuid().ToString());
        };
    }

    [Fact]
    public async Task GetRoles_ReturnOk()
    {
        var Role = await this.CreateRoleAsync();

        var response = await this.RequestAsync("http://localhost/api/Role", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var Roles = JsonSerializer.Deserialize<IEnumerable<RoleDto>>(json);

        Assert.NotNull(Roles);
        Assert.NotEmpty(Roles);
        Assert.Contains(Roles, x => x.Id == Role.Id);
    }

    [Fact]
    public async Task GetRoleById_ReturnOk()
    {
        var RoleCreated = await this.CreateRoleAsync();

        var response = await this.RequestAsync($"http://localhost/api/Role/{RoleCreated.Id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        var Role = JsonSerializer.Deserialize<RoleDto>(json);

        Assert.NotNull(Role);
        Assert.Equal(RoleCreated.Id, Role.Id);
        Assert.Equal(RoleCreated.Name, Role.Name);
        Assert.Equal(RoleCreated.Description, Role.Description);
    }

    [Fact]
    public async Task CreateRole_ReturnNoContent()
    {
        var data = new CreateRoleDto()
        {
            Id = Guid.NewGuid(),
            Name = "Role Test",
            Description = "Role Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Role",  content, HttpMethod.Post);

        var Role = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, Role.Id);
        Assert.Equal(data.Name, Role.Name);
        Assert.Equal(data.Description, Role.Description);
    }

    [Fact]
    public async Task UpdateRole_ReturnNoContent()
    {
        var RoleCreated = await this.CreateRoleAsync();

        var data = new UpdateRoleDto()
        {
            Id = RoleCreated.Id,
            Name = "Role Test Updated",
            Description = "Role Test Updated",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Role/{RoleCreated.Id}", content, HttpMethod.Put);

        var Role = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, Role.Id);
        Assert.Equal(data.Name, Role.Name);
        Assert.Equal(data.Description, Role.Description);
    }

    [Fact]
    public async Task DeleteRole_ReturnNoContent()
    {
        var RoleCreated = await this.CreateRoleAsync();

        var response = await this.RequestAsync($"http://localhost/api/Role/{RoleCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateRoleDto> CreateRoleAsync()
    {
        var data = new CreateRoleDto()
        {
            Id = Guid.NewGuid(),
            Name = "Role Test",
            Description = "Role Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Role", content, HttpMethod.Post);

        return data;
    }

    private async Task<RoleDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Role/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RoleDto>(json)!;
    }

    private async Task<HttpResponseMessage> RequestAsync(string uri, HttpContent? content, HttpMethod method)
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(uri),
            Content = content,
            Method = method
        };
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("TestAuth");

        var response = await Client.SendAsync(httpRequestMessage);

        return response;
    }

}
