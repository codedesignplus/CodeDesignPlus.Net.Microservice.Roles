namespace CodeDesignPlus.Net.Microservice.Roles.Application;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "200 : UnknownError";

    public const string InvalidRequest = "201 : The request is invalid.";
    public const string RoleAlreadyExists = "202 : The role already exists."; 
    public const string RoleNotFound = "203 : The role not found."; 
}
