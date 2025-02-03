namespace CodeDesignPlus.Net.Microservice.Roles.Domain;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "100 : UnknownError";

    public const string RoleIdIsInvalid = "101 : The role id is invalid.";
    public const string RoleNameIsInvalid = "102 : The role name is invalid.";
    public const string RoleDescriptionIsInvalid = "103 : The role description is invalid.";
    public const string CreatedByIsInvalid = "104 : The created by is invalid.";
}
