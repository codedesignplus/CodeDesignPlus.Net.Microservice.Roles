using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Roles.Application;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("200");

    public static readonly Error InvalidRequest = new("201");
    public static readonly Error RoleAlreadyExists = new("202"); 
    public static readonly Error RoleNotFound = new("203"); 
}
