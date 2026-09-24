using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Roles.Domain;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("100");

    public static readonly Error RoleIdIsInvalid = new("101");
    public static readonly Error RoleNameIsInvalid = new("102");
    public static readonly Error RoleDescriptionIsInvalid = new("103");
    public static readonly Error CreatedByIsInvalid = new("104");
}
