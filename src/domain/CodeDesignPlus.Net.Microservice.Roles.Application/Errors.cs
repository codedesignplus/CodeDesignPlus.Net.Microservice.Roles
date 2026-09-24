using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Roles.Application;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("200", "UnknownError");

    public static readonly Error InvalidRequest = new("201", "The request is invalid.");
    public static readonly Error RoleAlreadyExists = new("202", "The role already exists."); 
    public static readonly Error RoleNotFound = new("203", "The role not found."); 
}
