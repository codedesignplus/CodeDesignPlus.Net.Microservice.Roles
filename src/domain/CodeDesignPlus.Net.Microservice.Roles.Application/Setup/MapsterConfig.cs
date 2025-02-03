﻿using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.CreateRole;
using CodeDesignPlus.Net.Microservice.Roles.Application.Role.Commands.UpdateRole;

namespace CodeDesignPlus.Net.Microservice.Roles.Application.Setup;

public static class MapsterConfigRole
{
    public static void Configure()
    {

        //Role
        TypeAdapterConfig<CreateRoleDto, CreateRoleCommand>.NewConfig();
        TypeAdapterConfig<UpdateRoleDto, UpdateRoleCommand>.NewConfig();
        TypeAdapterConfig<RoleAggregate, RoleDto>.NewConfig();
    }
}
