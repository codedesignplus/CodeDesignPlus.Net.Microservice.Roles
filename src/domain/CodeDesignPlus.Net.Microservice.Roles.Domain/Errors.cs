using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Roles.Domain;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("100", "UnknownError");

    public static readonly Error RoleIdIsInvalid = new("101", "The role id is invalid.");
    public static readonly Error RoleNameIsInvalid = new("102", "The role name is invalid.");
    public static readonly Error RoleDescriptionIsInvalid = new("103", "The role description is invalid.");
    public static readonly Error CreatedByIsInvalid = new("104", "The created by is invalid.");
}
