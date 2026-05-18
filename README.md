# 🔑 Roles Microservice

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](LICENSE.md)
[![Tests](https://img.shields.io/badge/tests-passing-success)](tests/)
[![Coverage](https://img.shields.io/badge/coverage-80%25-green)]()
[![Docker](https://img.shields.io/badge/docker-ready-2496ED?logo=docker)](Dockerfile)

A production-ready microservice for managing roles and permissions in RBAC (Role-Based Access Control) systems built with .NET 9. Implements Clean Architecture, DDD, and CQRS patterns with support for role lifecycle management and permission assignment.

---

## What is this microservice?

The Roles microservice defines the types of users that exist in the platform (Administrator, Accountant, Property Owner, Resident, Security Guard, etc.) and provides a catalog of these roles for each organization. It solves the problem of determining "what categories of users exist and what should they be allowed to do" at a definition level. Platform administrators use it to create and customize roles per organization. It feeds into the RBAC microservice, which maps these roles to specific permissions, and into the Users microservice, which assigns roles to individual people.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#️-technology-stack)
- [Prerequisites](#️-prerequisites)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [Role Model](#-role-model)
- [Configuration](#️-configuration)
- [Use Cases & Scenarios](#-use-cases--scenarios)
- [Architecture](#️-architecture)
- [Testing](#-testing)
- [Best Practices](#-best-practices)
- [Troubleshooting](#-troubleshooting)
- [RBAC Integration](#-rbac-integration)
- [Domain Events](#-domain-events)
- [FAQ](#-faq)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

The Roles microservice provides a centralized system for managing roles in an RBAC security model. It's designed for multi-tenant applications, enterprise systems, and platforms requiring fine-grained access control.

- **Role Management**: Create, update, and delete roles with rich metadata
- **Permission System**: Define and assign permissions to roles for granular access control
- **Multi-Tenancy**: Isolate roles by organization or tenant
- **Active/Inactive States**: Control role availability without deletion
- **Soft Deletion**: Maintain audit trails with logical deletes
- **Domain Events**: Publish role lifecycle events for downstream integration
- **CQRS Pattern**: Optimized read and write operations
- **Audit Trail**: Track who created, updated, or deleted each role

### 🚀 Quick Start

```bash
# 1. Start infrastructure services
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d

# 2. Configure Vault secrets
cd ../../tools/vault
./config-vault.sh

# 3. Run the microservice
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Roles.Rest

# 4. Access Swagger UI
open http://localhost:5000/swagger
```

### 📊 High-Level Architecture

```
┌─────────────┐
│   Client    │
│ Application │
└──────┬──────┘
       │ HTTPS + JWT
       │
┌──────▼──────────────────────────────────────────────┐
│         Roles Microservice (REST API)               │
│  ┌──────────────┐  ┌─────────────┐  ┌────────────┐ │
│  │ Controllers  │  │  MediatR    │  │  Handlers  │ │
│  │   (API)      │─▶│   (CQRS)    │─▶│ (Business) │ │
│  └──────────────┘  └─────────────┘  └────┬───────┘ │
│                                           │         │
│  ┌────────────────────────────────────────▼──────┐ │
│  │         RoleAggregate (Domain)                │ │
│  │  ┌──────────────────────────────────────────┐ │ │
│  │  │  • Create  • Update  • Delete            │ │ │
│  │  │  • Domain Events  • Business Rules       │ │ │
│  │  └──────────────────────────────────────────┘ │ │
│  └────────────────────────────────────────────────┘ │
└───────┬──────────────────┬──────────────────┬───────┘
        │                  │                  │
   ┌────▼────┐      ┌──────▼──────┐    ┌─────▼─────┐
   │ MongoDB │      │   Redis     │    │ RabbitMQ  │
   │ (Roles) │      │  (Cache)    │    │ (Events)  │
   └─────────┘      └─────────────┘    └───────────┘
```

## 🚀 Key Features

### Core Capabilities

- ✅ **CRUD Operations**: Create, read, update, and delete roles with full lifecycle management
- ✅ **Rich Metadata**: Name, description, active status, and audit fields
- ✅ **Soft Deletion**: Logical deletes preserve audit trails and data integrity
- ✅ **Active/Inactive Toggle**: Control role availability without deletion
- ✅ **Multi-Tenancy**: Tenant-aware queries and operations via `X-Tenant` header
- ✅ **Domain Events**: Publish RoleCreated, RoleUpdated, RoleDeleted events
- ✅ **Pagination & Filtering**: OData-style criteria for role listing
- ✅ **Audit Trail**: CreatedBy, CreatedAt, UpdatedBy, UpdatedAt, DeletedBy, DeletedAt
- ✅ **Validation**: FluentValidation rules for name (max 128), description (max 512)
- ✅ **Problem Details**: RFC 7807 compliant error responses

### Technical Features

- Clean Architecture with DDD and CQRS
- Domain events for state changes
- MongoDB for role persistence
- RabbitMQ for event publishing
- Redis for distributed caching
- OAuth2/OpenID Connect security
- Multi-tenancy support
- Swagger/OpenAPI documentation
- Docker containerization
- Comprehensive test coverage (Unit, Integration)

## 🛠️ Technology Stack

### Core
- **.NET 9** - Runtime and framework
- **ASP.NET Core** - Web API framework
- **C# 13** - Programming language

### Storage & Data
- **MongoDB** - Role persistence and queries
- **Redis** - Distributed caching and session storage

### Messaging & Events
- **RabbitMQ** - Event publishing and message broker

### Architecture & Patterns
- **MediatR** - CQRS command/query handling
- **FluentValidation** - Input validation
- **Mapster** - Object mapping
- **NodaTime** - Date/time handling

### Security & Configuration
- **Vault** - Secret management
- **OAuth2/OpenID Connect** - Authentication
- **JWT Bearer** - Token-based security
- **HTTPS** - Encrypted communication

### DevOps & Testing
- **Docker** - Containerization
- **xUnit** - Unit/integration testing
- **Swagger/OpenAPI** - API documentation

## ⚙️ Prerequisites

### Required
- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker & Docker Compose** - For infrastructure services
- **MongoDB 6.0+** - Document database
- **Redis 7.0+** - Caching layer
- **RabbitMQ 3.12+** - Message broker

### Optional
- **Vault** - Secret management (can use appsettings for local dev)
- **RBAC Microservice** - For permission validation (configured via `Security:ServerRbac`)

## 🚀 Getting Started

The following instructions will help you set up the project on your local machine for development and testing purposes.

1. Clone the repository:
```bash
git clone <repository-url>
cd CodeDesignPlus.Net.Microservice.Roles
```

2. Run the MongoDB, Redis, and RabbitMQ services using Docker Compose. Clone this repository [CodeDesignPlus.Environment.Dev](https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev) and run the following command:

```bash
cd resources
docker-compose up -d
```

3. Run the script to config the vault:

```bash
cd tools/vault
./config-vault.sh
```

4. Build the solution:
```bash
dotnet build
```

5. Run the REST API entrypoint:
   
```bash
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Roles.Rest
```

## 📡 API Endpoints

### Role Operations

#### Get All Roles (Paginated)
```http
GET /api/role?limit=50&skip=0&filter=isActive eq true&orderby=name asc
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Query Parameters**:
- `limit` (optional): Number of items per page (default: 100)
- `skip` (optional): Number of items to skip (default: 0)
- `filter` (optional): OData filter expression (e.g., `isActive eq true`, `name eq 'Admin'`)
- `orderby` (optional): OData order expression (e.g., `name asc`, `createdAt desc`)

**Response**: `200 OK` with paginated results
```json
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "Administrator",
      "description": "Full system access with all permissions",
      "isActive": true
    },
    {
      "id": "660e8400-e29b-41d4-a716-446655440001",
      "name": "Editor",
      "description": "Can create and edit content",
      "isActive": true
    }
  ],
  "totalCount": 15,
  "limit": 50,
  "skip": 0
}
```

#### Get Role by ID
```http
GET /api/role/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `200 OK` with role details
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Administrator",
  "description": "Full system access with all permissions",
  "isActive": true
}
```

#### Create Role
```http
POST /api/role
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Administrator",
  "description": "Full system access with all permissions"
}
```

**Validation Rules**:
- `id`: Required, must be a valid GUID
- `name`: Required, max 128 characters
- `description`: Required, max 512 characters

**Response**: `204 No Content` on success

**Business Rules**:
- Role ID must be unique (returns `400 Bad Request` if exists)
- Role is created with `isActive = true` by default
- `createdBy` is automatically set from JWT token (`IUserContext`)
- `createdAt` is set to current UTC time using `NodaTime.SystemClock`

#### Update Role
```http
PUT /api/role/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "Super Administrator",
  "description": "Enhanced admin role with additional privileges",
  "isActive": true
}
```

**Validation Rules**:
- `id`: Required, must be a valid GUID (from URL)
- `name`: Required, max 128 characters
- `description`: Required, max 512 characters
- `isActive`: Required, boolean

**Response**: `204 No Content` on success

**Business Rules**:
- Role must exist (returns `404 Not Found` if not found)
- `updatedBy` is automatically set from JWT token
- `updatedAt` is set to current UTC time
- Domain event `RoleUpdatedDomainEvent` is published

#### Delete Role
```http
DELETE /api/role/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content` on success

**Business Rules**:
- Performs soft delete (sets `isDeleted = true`, `isActive = false`)
- Role must exist (returns `404 Not Found` if not found)
- `deletedBy` is automatically set from JWT token
- `deletedAt` is set to current UTC time
- Domain event `RoleDeletedDomainEvent` is published
- Deleted roles are excluded from normal queries

### Error Responses

All errors follow RFC 7807 Problem Details format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Role not found.",
  "extensions": {
    "layer": "Application",
    "error_code": "ROLE-404",
    "traceId": "0HMVJ3K7S5Q2K:00000001"
  }
}
```

**Common Status Codes**:
- `200 OK` - Successful query
- `204 No Content` - Successful command (create/update/delete)
- `400 Bad Request` - Invalid input or business rule violation
- `401 Unauthorized` - Missing or invalid token
- `404 Not Found` - Role not found
- `500 Internal Server Error` - Server error

## 🎭 Role Model

### RoleAggregate (Domain)

#### What is it and what is it for?

The RoleAggregate represents a named category of user (e.g., "Administrator," "Resident," "Accountant") within an organization. It holds the role's name, description, and active status. Roles are the building blocks of the access control system: they are assigned to users and then mapped to permissions via the RBAC microservice to determine what each person can do.

The `RoleAggregate` is the core domain entity that encapsulates role business logic:

```csharp
public class RoleAggregate : AggregateRootBase
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    
    // Audit fields (inherited from AggregateRootBase)
    public Guid CreatedBy { get; private set; }
    public Instant CreatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public Instant? UpdatedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }
    public Instant? DeletedAt { get; private set; }
}
```

### Domain Rules

The aggregate enforces these invariants:

1. **Role ID**: Must be a valid, non-empty GUID
2. **Name**: Cannot be null or empty, max 128 characters
3. **Description**: Cannot be null or empty, max 512 characters
4. **Creation**: Requires a valid `createdBy` user ID
5. **Update**: Requires a valid `updatedBy` user ID
6. **Deletion**: Requires a valid `deletedBy` user ID
7. **State**: New roles are `isActive = true`, deleted roles are `isActive = false`

### RoleDto (Application)

DTOs used for API requests and responses:

```csharp
// Query response
public class RoleDto : IDtoBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}

// Create request
public class CreateRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

// Update request
public class UpdateRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
```

**Note**: DTOs are generated via `[DtoGenerator]` attribute on commands using `CodeDesignPlus.Net.Generator`.

## ⚙️ Configuration

### Core Configuration

Configure the microservice in `appsettings.json`:

```json
{
  "Core": {
    "Id": "ab529c76-affc-4e46-b137-ba56c3df0101",
    "PathBase": "/ms-roles",
    "AppName": "ms-roles",
    "TypeEntryPoint": "rest",
    "Version": "v1",
    "Description": "Microservice to manage Roles",
    "Business": "CodeDesignPlus",
    "Contact": {
      "Name": "CodeDesignPlus",
      "Email": "support@codedesignplus.com"
    }
  }
}
```

### MongoDB Configuration

```json
{
  "Mongo": {
    "Enable": true,
    "Database": "db-ms-roles",
    "Diagnostic": {
      "Enable": false,
      "EnableCommandText": false
    }
  }
}
```

**Connection String**: Managed by Vault in production, or set via:
```json
{
  "Vault": {
    "Mongo": {
      "Enable": true,
      "TemplateConnectionString": "mongodb://{0}:{1}@localhost:27017"
    }
  }
}
```

### Security Configuration

```json
{
  "Security": {
    "IncludeErrorDetails": true,
    "ValidateAudience": true,
    "ValidateIssuer": true,
    "ValidateLifetime": true,
    "RequireHttpsMetadata": true,
    "ValidIssuer": "https://your-identity-server.com",
    "ValidAudiences": ["roles-api"],
    "Applications": [],
    "ValidateLicense": false,
    "ValidateRbac": false,
    "ServerRbac": "http://localhost:5001",
    "RefreshRbacInterval": 10
  }
}
```

**RBAC Integration**:
- `ValidateRbac`: Set to `true` to enable permission checks against RBAC microservice
- `ServerRbac`: URL of RBAC microservice (e.g., `http://ms-rbac.codedesignplus.svc.cluster.local:5001`)
- `RefreshRbacInterval`: Cache refresh interval in seconds

### Redis Caching

```json
{
  "Redis": {
    "Instances": {
      "Core": {
        "ConnectionString": "localhost:6379"
      }
    }
  },
  "RedisCache": {
    "Enable": true,
    "Expiration": "00:05:00"
  }
}
```

### RabbitMQ Events

```json
{
  "RabbitMQ": {
    "Enable": true,
    "Host": "localhost",
    "Port": 5672,
    "UserName": "user",
    "Password": "pass",
    "EnableDiagnostic": false
  }
}
```

### Vault Integration

```json
{
  "Vault": {
    "Enable": true,
    "Address": "http://localhost:8200",
    "AppName": "ms-roles",
    "Solution": "security-codedesignplus",
    "Token": "root",
    "Mongo": {
      "Enable": true,
      "TemplateConnectionString": "mongodb://{0}:{1}@localhost:27017"
    },
    "RabbitMQ": {
      "Enable": true
    }
  }
}
```

**Vault Secrets Path**: `{Solution}/{AppName}/*`
- MongoDB credentials: `security-codedesignplus/ms-roles/mongo`
- RabbitMQ credentials: `security-codedesignplus/ms-roles/rabbitmq`

### Multi-tenancy

The microservice supports multi-tenancy through the `X-Tenant` header. Each request must include a tenant ID:

```http
X-Tenant: 9588813a-7bc0-4be4-a169-293061881cc3
```

Roles are isolated by tenant at the repository level via `IUserContext.Tenant`.

### Environment Variables

Key environment variables for Docker deployment:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
Vault__Enable=true
Vault__Address=http://vault.codedesignplus.svc.cluster.local:8200
Vault__Token=<vault-token>
```

## 💼 Use Cases & Scenarios

### 1. Role-Based Access Control (RBAC)

**Scenario**: A SaaS platform needs to manage user roles (Admin, Editor, Viewer) with different permission sets.

**Implementation**:
```bash
# Create Admin role
POST /api/role
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Administrator",
  "description": "Full system access with all permissions"
}

# Create Editor role
POST /api/role
{
  "id": "660e8400-e29b-41d4-a716-446655440001",
  "name": "Editor",
  "description": "Can create and edit content"
}

# Create Viewer role
POST /api/role
{
  "id": "770e8400-e29b-41d4-a716-446655440002",
  "name": "Viewer",
  "description": "Read-only access to content"
}
```

**Integration**:
- Roles are consumed by the RBAC microservice to assign permissions
- User microservice assigns roles to users
- API Gateway validates permissions via RBAC service

### 2. Multi-Tenant Organization Roles

**Scenario**: A multi-tenant system where each organization has its own set of roles.

**Implementation**:
```bash
# Tenant A - Create roles
X-Tenant: org-a-tenant-id
POST /api/role
{
  "id": "...",
  "name": "Org Admin",
  "description": "Organization administrator"
}

# Tenant B - Create roles (isolated from Tenant A)
X-Tenant: org-b-tenant-id
POST /api/role
{
  "id": "...",
  "name": "Org Admin",
  "description": "Organization administrator"
}
```

**Benefits**:
- Complete role isolation between tenants
- Each organization can define custom roles
- No role name conflicts across tenants

### 3. Role Lifecycle Management

**Scenario**: Deactivate a role temporarily without losing audit history.

**Implementation**:
```bash
# Deactivate role
PUT /api/role/550e8400-e29b-41d4-a716-446655440000
{
  "name": "Administrator",
  "description": "Full system access",
  "isActive": false
}

# Later: Reactivate role
PUT /api/role/550e8400-e29b-41d4-a716-446655440000
{
  "name": "Administrator",
  "description": "Full system access",
  "isActive": true
}

# Soft delete (preserves audit trail)
DELETE /api/role/550e8400-e29b-41d4-a716-446655440000
```

**Audit Trail**:
- `createdBy`, `createdAt`: Who created the role and when
- `updatedBy`, `updatedAt`: Who last modified the role
- `deletedBy`, `deletedAt`: Who deleted the role (soft delete)

### 4. Event-Driven Role Synchronization

**Scenario**: Sync role changes to downstream services (User service, RBAC service, Analytics).

**Domain Events Published**:
```json
// RoleCreatedDomainEvent
{
  "aggregateId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Administrator",
  "description": "Full system access",
  "isActive": true,
  "eventId": "...",
  "occurredAt": "2026-05-15T10:00:00Z"
}

// RoleUpdatedDomainEvent
{
  "aggregateId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Super Administrator",
  "description": "Enhanced admin role",
  "isActive": true,
  "eventId": "...",
  "occurredAt": "2026-05-15T11:00:00Z"
}

// RoleDeletedDomainEvent
{
  "aggregateId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Administrator",
  "description": "Full system access",
  "isActive": false,
  "eventId": "...",
  "occurredAt": "2026-05-15T12:00:00Z"
}
```

**Consumers**:
- **RBAC Service**: Update permission assignments when role changes
- **User Service**: Refresh user role cache
- **Analytics Service**: Track role usage and changes

### 5. Filtered Role Queries

**Scenario**: Admin UI needs to display only active roles, sorted by name.

**Implementation**:
```bash
# Get all active roles
GET /api/role?filter=isActive eq true&orderby=name asc

# Get roles matching name pattern
GET /api/role?filter=contains(name,'Admin')&limit=20

# Paginate through roles
GET /api/role?skip=0&limit=50
GET /api/role?skip=50&limit=50
```

**OData Operators**:
- `eq` (equals): `isActive eq true`
- `ne` (not equals): `isActive ne false`
- `contains`: `contains(name,'Admin')`
- `startswith`: `startswith(name,'Admin')`
- `endswith`: `endswith(name,'Admin')`

## 🏗️ Architecture

### Clean Architecture Layers

```
src/
├── domain/
│   ├── CodeDesignPlus.Net.Microservice.Roles.Domain/
│   │   ├── RoleAggregate.cs                    # Aggregate root
│   │   ├── DomainEvents/                       # Domain events
│   │   │   ├── RoleCreatedDomainEvent.cs
│   │   │   ├── RoleUpdatedDomainEvent.cs
│   │   │   └── RoleDeletedDomainEvent.cs
│   │   ├── Repositories/
│   │   │   └── IRoleRepository.cs              # Repository interface
│   │   ├── Errors.cs                           # Error codes
│   │   └── Startup.cs                          # DI registration
│   │
│   ├── CodeDesignPlus.Net.Microservice.Roles.Application/
│   │   ├── Role/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateRole/
│   │   │   │   │   ├── CreateRoleCommand.cs    # Command + Validator
│   │   │   │   │   └── CreateRoleCommandHandler.cs
│   │   │   │   ├── UpdateRole/
│   │   │   │   │   ├── UpdateRoleCommand.cs
│   │   │   │   │   └── UpdateRoleCommandHandler.cs
│   │   │   │   └── DeleteRole/
│   │   │   │       ├── DeleteRoleCommand.cs
│   │   │   │       └── DeleteRoleCommandHandler.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetAllRole/
│   │   │   │   │   ├── GetAllRoleQuery.cs
│   │   │   │   │   └── GetAllRoleQueryHandler.cs
│   │   │   │   └── GetRoleById/
│   │   │   │       ├── GetRoleByIdQuery.cs
│   │   │   │       └── GetRoleByIdQueryHandler.cs
│   │   │   └── DataTransferObjects/
│   │   │       └── RoleDto.cs                  # DTOs
│   │   └── Startup.cs                          # DI registration
│   │
│   └── CodeDesignPlus.Net.Microservice.Roles.Infrastructure/
│       ├── Repositories/
│       │   └── RoleRepository.cs               # MongoDB implementation
│       ├── Errors.cs                           # Error codes
│       └── Startup.cs                          # DI registration
│
└── entrypoints/
    └── CodeDesignPlus.Net.Microservice.Roles.Rest/
        ├── Controllers/
        │   └── RoleController.cs               # REST endpoints
        ├── Program.cs                          # Entry point
        ├── appsettings.json                    # Configuration
        └── Dockerfile                          # Container image
```

### Request Flow (CQRS)

#### Command Flow (Write)
```
HTTP POST/PUT/DELETE
    ↓
RoleController
    ↓ (IMediator.Send)
CreateRoleCommand + Validator (FluentValidation)
    ↓
CreateRoleCommandHandler
    ↓ (ApplicationGuard checks)
RoleAggregate.Create() (DomainGuard invariants)
    ↓ (AddEvent)
RoleCreatedDomainEvent
    ↓
IRoleRepository.CreateAsync() (MongoDB)
    ↓
IPubSub.PublishAsync() (RabbitMQ)
    ↓
204 No Content
```

#### Query Flow (Read)
```
HTTP GET
    ↓
RoleController
    ↓ (IMediator.Send)
GetAllRoleQuery
    ↓
GetAllRoleQueryHandler
    ↓ (IUserContext.Tenant)
IRoleRepository.FindAsync(criteria) (MongoDB)
    ↓ (Mapster)
Pagination<RoleDto>
    ↓
200 OK + JSON
```

### Domain-Driven Design

#### Aggregate Root: RoleAggregate

**Responsibilities**:
- Enforce role invariants (valid ID, name, description)
- Manage role lifecycle (create, update, delete)
- Publish domain events
- Track audit information

**Factory Method**:
```csharp
public static RoleAggregate Create(
    Guid id, 
    string name, 
    string description, 
    Guid createBy)
{
    // Domain guards ensure invariants
    DomainGuard.GuidIsEmpty(id, Errors.RoleIdIsInvalid);
    DomainGuard.IsNullOrEmpty(name, Errors.RoleNameIsInvalid);
    DomainGuard.IsNullOrEmpty(description, Errors.RoleDescriptionIsInvalid);
    DomainGuard.GuidIsEmpty(createBy, Errors.CreatedByIsInvalid);

    return new RoleAggregate(id, name, description, createBy);
}
```

**Business Methods**:
- `Update(name, description, isActive, updatedBy)`: Update role details
- `Delete(deletedBy)`: Soft delete the role

#### Domain Events

Events are published after successful persistence:

1. **RoleCreatedDomainEvent**: Published when a new role is created
2. **RoleUpdatedDomainEvent**: Published when role details are modified
3. **RoleDeletedDomainEvent**: Published when role is soft-deleted

**Event Structure**:
```csharp
[EventKey<RoleAggregate>(1, "RoleCreatedDomainEvent")]
public class RoleCreatedDomainEvent : DomainEvent
{
    public Guid AggregateId { get; }
    public string Name { get; }
    public string Description { get; }
    public bool IsActive { get; }
    public Instant OccurredAt { get; }
}
```

#### Repository Pattern

**Interface** (`IRoleRepository`):
```csharp
public interface IRoleRepository : IRepositoryBase
{
    // Inherits from IRepositoryBase:
    // - CreateAsync<T>()
    // - UpdateAsync<T>()
    // - DeleteAsync<T>()
    // - FindAsync<T>()
    // - ExistsAsync<T>()
}
```

**Implementation** (`RoleRepository`):
- Extends `RepositoryBase` from CodeDesignPlus.Net.Mongo
- Automatic tenant filtering via `IUserContext`
- MongoDB collection: `roles`
- Indexes: `{ id: 1, tenant: 1 }`, `{ name: 1, tenant: 1 }`

### CQRS Pattern

**Commands** (Write Operations):
- `CreateRoleCommand`: Create new role
- `UpdateRoleCommand`: Update existing role
- `DeleteRoleCommand`: Soft delete role

**Queries** (Read Operations):
- `GetAllRoleQuery`: Paginated role listing with filtering
- `GetRoleByIdQuery`: Single role by ID

**Benefits**:
- Optimized read/write paths
- Clear separation of concerns
- Easy to add caching on read side
- Write side focused on business rules

## 🧪 Testing

### Test Structure

```
tests/
├── unit/
│   ├── CodeDesignPlus.Net.Microservice.Roles.Domain.Test/
│   │   ├── RoleAggregateTest.cs
│   │   └── DomainEvents/
│   ├── CodeDesignPlus.Net.Microservice.Roles.Application.Test/
│   │   └── Role/
│   │       ├── Commands/
│   │       │   └── CreateRole/
│   │       │       └── CreateRoleCommandHandlerTest.cs
│   │       └── Queries/
│   └── CodeDesignPlus.Net.Microservice.Roles.Infrastructure.Test/
│       └── Repositories/
│           └── RoleRepositoryTest.cs
│
└── integration/
    └── CodeDesignPlus.Net.Microservice.Roles.Rest.Test/
        └── Controller/
            └── RoleControllerTest.cs
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/unit/CodeDesignPlus.Net.Microservice.Roles.Domain.Test

# Run specific test class
dotnet test --filter "FullyQualifiedName~RoleAggregateTest"

# Run specific test method
dotnet test --filter "FullyQualifiedName~RoleAggregateTest.Create_ValidParameters_ShouldCreateRole"

# Generate coverage report
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Integration Tests

Integration tests use `WebApplicationFactory` to test the full request pipeline:

```csharp
public class RoleControllerTest : ServerBase<Program>
{
    [Fact]
    public async Task CreateRole_ReturnNoContent()
    {
        var data = new CreateRoleDto
        {
            Id = Guid.NewGuid(),
            Name = "Test Role",
            Description = "Test Description"
        };

        var response = await Client.PostAsJsonAsync("/api/role", data);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
```

**Test Infrastructure**:
- In-memory configuration (Vault disabled)
- TestAuth authentication handler
- MongoDB test database (isolated per test)
- RabbitMQ mocked via in-memory queue

### Unit Tests

Unit tests focus on isolated business logic:

```csharp
public class RoleAggregateTest
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateRole()
    {
        var id = Guid.NewGuid();
        var name = "Administrator";
        var description = "Full access";
        var createdBy = Guid.NewGuid();

        var role = RoleAggregate.Create(id, name, description, createdBy);

        Assert.Equal(id, role.Id);
        Assert.Equal(name, role.Name);
        Assert.Equal(description, role.Description);
        Assert.True(role.IsActive);
        Assert.Single(role.GetDomainEvents());
    }

    [Fact]
    public void Create_EmptyName_ShouldThrowException()
    {
        var ex = Assert.Throws<CoreException>(() =>
            RoleAggregate.Create(Guid.NewGuid(), "", "desc", Guid.NewGuid())
        );

        Assert.Contains("RoleNameIsInvalid", ex.Message);
    }
}
```

## 🎯 Best Practices

### 1. Always Use Tenant Header

```csharp
// ✅ Correct
var request = new HttpRequestMessage(HttpMethod.Get, "/api/role");
request.Headers.Add("X-Tenant", tenantId);

// ❌ Incorrect (missing tenant)
var request = new HttpRequestMessage(HttpMethod.Get, "/api/role");
```

### 2. Use Specific Error Codes

```csharp
// Domain errors
public const string RoleIdIsInvalid = "101 : The role id is invalid.";
public const string RoleNameIsInvalid = "102 : The role name is invalid.";
public const string RoleDescriptionIsInvalid = "103 : The role description is invalid.";

// Application errors
public const string RoleAlreadyExists = "201 : The role already exists.";
public const string RoleNotFound = "202 : The role not found.";
```

### 3. Validate Before Persistence

```csharp
// ✅ Correct: Domain validation + Application checks
var exist = await repository.ExistsAsync<RoleAggregate>(request.Id, cancellationToken);
ApplicationGuard.IsTrue(exist, Errors.RoleAlreadyExists);

var role = RoleAggregate.Create(request.Id, request.Name, request.Description, user.IdUser);
await repository.CreateAsync(role, cancellationToken);

// ❌ Incorrect: No existence check
var role = RoleAggregate.Create(request.Id, request.Name, request.Description, user.IdUser);
await repository.CreateAsync(role, cancellationToken); // May fail with duplicate key
```

### 4. Always Publish Domain Events

```csharp
// ✅ Correct: Publish events after persistence
await repository.CreateAsync(role, cancellationToken);
await pubsub.PublishAsync(role.GetAndClearEvents(), cancellationToken);

// ❌ Incorrect: Events not published
await repository.CreateAsync(role, cancellationToken);
// Missing event publishing - downstream services won't be notified
```

### 5. Use Soft Deletes for Audit Trail

```csharp
// ✅ Correct: Soft delete preserves audit
role.Delete(user.IdUser); // Sets isDeleted=true, isActive=false
await repository.UpdateAsync(role, cancellationToken);

// ❌ Incorrect: Hard delete loses audit trail
await repository.DeleteAsync<RoleAggregate>(id, cancellationToken);
```

### 6. Leverage CQRS for Performance

```csharp
// ✅ Correct: Use queries for reads
var query = new GetAllRoleQuery(criteria);
var result = await mediator.Send(query, cancellationToken);

// Add caching on read side
var cacheKey = $"roles:{tenant}:{criteria.GetHashCode()}";
var cached = await cache.GetAsync<Pagination<RoleDto>>(cacheKey);
if (cached != null) return cached;
```

### 7. Configuration Management

```bash
# ✅ Development: Use appsettings.Development.json
dotnet run --environment Development

# ✅ Production: Use Vault for secrets
Vault__Enable=true
Vault__Address=http://vault:8200

# ❌ Never commit secrets to appsettings.json
"Mongo": {
  "ConnectionString": "mongodb://admin:secret@prod:27017" // ❌ NEVER!
}
```

## 🔧 Troubleshooting

### Common Issues

#### 1. 401 Unauthorized on All Requests

**Symptoms**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

**Causes & Solutions**:
- **Missing JWT token**: Add `Authorization: Bearer {token}` header
- **Invalid issuer**: Check `Security:ValidIssuer` matches your identity provider
- **Invalid audience**: Ensure token audience matches `Security:ValidAudiences`
- **Expired token**: Refresh the access token

**Debug**:
```bash
# Enable detailed security logs
"Logger": {
  "Level": "Debug"
}

# Check token claims
jwt.io # Paste your token to inspect claims
```

#### 2. Vault Connection Failures

**Symptoms**:
```
Unable to connect to Vault server at http://localhost:8200
```

**Solutions**:
```bash
# 1. Verify Vault is running
docker ps | grep vault

# 2. Check Vault configuration
curl http://localhost:8200/v1/sys/health

# 3. Disable Vault for local dev
"Vault": {
  "Enable": false
}

# 4. Verify Vault token is valid
vault token lookup
```

#### 3. MongoDB Connection Errors

**Symptoms**:
```
MongoConnectionException: Unable to connect to server localhost:27017
```

**Solutions**:
```bash
# 1. Verify MongoDB is running
docker ps | grep mongo

# 2. Check connection string
"Mongo": {
  "ConnectionString": "mongodb://localhost:27017"
}

# 3. Test connection manually
mongosh mongodb://localhost:27017

# 4. Check network (Docker)
docker network ls
docker network inspect backend
```

#### 4. RabbitMQ Publishing Failures

**Symptoms**:
```
RabbitMQ.Client.Exceptions.BrokerUnreachableException
```

**Solutions**:
```bash
# 1. Verify RabbitMQ is running
docker ps | grep rabbitmq

# 2. Check RabbitMQ management UI
open http://localhost:15672 # guest/guest

# 3. Disable RabbitMQ for testing
"RabbitMQ": {
  "Enable": false
}

# 4. Check credentials in Vault
vault kv get security-codedesignplus/ms-roles/rabbitmq
```

#### 5. Role Already Exists Error

**Symptoms**:
```json
{
  "status": 400,
  "detail": "The role already exists.",
  "error_code": "201"
}
```

**Cause**: Attempting to create a role with a duplicate ID.

**Solutions**:
- Generate a new GUID for the role
- Check if role exists before creation: `GET /api/role/{id}`
- Delete existing role if testing: `DELETE /api/role/{id}`

#### 6. Tenant Isolation Issues

**Symptoms**: Roles from one tenant visible to another tenant.

**Cause**: Missing or incorrect `X-Tenant` header.

**Solutions**:
```bash
# ✅ Always include tenant header
curl -H "X-Tenant: org-a-id" http://localhost:5000/api/role

# ❌ Missing tenant
curl http://localhost:5000/api/role # Will use default or fail

# Verify tenant in JWT
# IUserContext extracts tenant from token claims
```

### Debugging Tips

#### Enable Detailed Logging

```json
{
  "Logger": {
    "Enable": true,
    "Level": "Debug",
    "OTelEndpoint": "http://localhost:4317"
  },
  "Observability": {
    "Enable": true,
    "Trace": {
      "Enable": true,
      "AspNetCore": true,
      "CodeDesignPlusSdk": true,
      "Redis": true,
      "RabbitMQ": true
    }
  }
}
```

#### Inspect MongoDB Collections

```bash
# Connect to MongoDB
mongosh mongodb://localhost:27017

# Switch to roles database
use db-ms-roles

# List all roles
db.roles.find()

# Find specific role
db.roles.find({ "id": "550e8400-e29b-41d4-a716-446655440000" })

# Check tenant isolation
db.roles.find({ "tenant": "org-a-id" })
```

#### Test with Swagger UI

```bash
# 1. Run the microservice
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Roles.Rest

# 2. Open Swagger
open http://localhost:5000/swagger

# 3. Authorize (if security enabled)
Click "Authorize" → Enter Bearer token

# 4. Test endpoints interactively
```

#### Check Health Endpoints

```bash
# Liveness probe
curl http://localhost:5000/health/live

# Readiness probe
curl http://localhost:5000/health/ready

# Response
{
  "status": "Healthy",
  "checks": [
    { "name": "mongo", "status": "Healthy" },
    { "name": "redis", "status": "Healthy" },
    { "name": "rabbitmq", "status": "Healthy" }
  ]
}
```

## 🔐 RBAC Integration

The Roles microservice is a core component of the RBAC (Role-Based Access Control) system.

### RBAC Architecture

```
┌─────────────┐
│   User      │
│ Microservice│
└──────┬──────┘
       │ Assigns Roles
       │ to Users
┌──────▼──────┐
│   Roles     │ ← You are here
│ Microservice│
└──────┬──────┘
       │ Roles have
       │ Permissions
┌──────▼──────┐
│   RBAC      │
│ Microservice│
└──────┬──────┘
       │ Validates
       │ Access
┌──────▼──────┐
│ API Gateway │
│  + Services │
└─────────────┘
```

### Integration Flow

#### 1. Role Creation
```bash
# 1. Create role in Roles microservice
POST /api/role
{
  "id": "admin-role-id",
  "name": "Administrator",
  "description": "Full system access"
}

# 2. RoleCreatedDomainEvent published to RabbitMQ
# 3. RBAC microservice consumes event and creates role record
# 4. RBAC microservice assigns default permissions to role
```

#### 2. User Role Assignment
```bash
# User microservice assigns role to user
POST /api/user/550e8400.../roles
{
  "roleIds": ["admin-role-id"]
}

# RBAC microservice caches: user -> roles -> permissions
```

#### 3. Permission Validation
```bash
# API Gateway checks permission
Authorization: Bearer {jwt-token}
X-Tenant: org-id

# 1. Extract user ID from JWT
# 2. Call RBAC microservice: GET /api/rbac/validate
# 3. RBAC returns: { "hasPermission": true, "roles": [...] }
# 4. Gateway allows/denies request
```

### RBAC Configuration

Enable RBAC validation in `appsettings.json`:

```json
{
  "Security": {
    "ValidateRbac": true,
    "ServerRbac": "http://ms-rbac.codedesignplus.svc.cluster.local:5001",
    "RefreshRbacInterval": 10
  }
}
```

**How it works**:
- On each request, `IUserContext` calls RBAC service to validate permissions
- Results are cached in Redis for `RefreshRbacInterval` seconds
- Cache key: `rbac:{tenant}:{userId}:{resource}:{action}`

### Permission Model

Roles don't store permissions directly. Permissions are managed by RBAC microservice:

```json
// RBAC microservice: Permission assignment
{
  "roleId": "admin-role-id",
  "permissions": [
    { "resource": "roles", "actions": ["create", "read", "update", "delete"] },
    { "resource": "users", "actions": ["create", "read", "update", "delete"] },
    { "resource": "payments", "actions": ["read", "approve", "refund"] }
  ]
}
```

### Custom RBAC Logic

To add custom authorization logic:

```csharp
[Authorize]
[RequirePermission("roles", "create")]
public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto data)
{
    // IUserContext automatically validates permission
    // via [RequirePermission] attribute
    
    await mediator.Send(mapper.Map<CreateRoleCommand>(data));
    return NoContent();
}
```

## 📨 Domain Events

All role state changes publish domain events to RabbitMQ for downstream integration.

### Event Schema

#### RoleCreatedDomainEvent

**Published when**: New role is created via `POST /api/role`

**Schema**:
```json
{
  "eventType": "RoleCreatedDomainEvent",
  "eventId": "event-guid",
  "aggregateId": "role-guid",
  "occurredAt": "2026-05-15T10:00:00Z",
  "metadata": {
    "tenant": "org-id",
    "userId": "user-id"
  },
  "data": {
    "name": "Administrator",
    "description": "Full system access",
    "isActive": true
  }
}
```

#### RoleUpdatedDomainEvent

**Published when**: Role is updated via `PUT /api/role/{id}`

**Schema**:
```json
{
  "eventType": "RoleUpdatedDomainEvent",
  "eventId": "event-guid",
  "aggregateId": "role-guid",
  "occurredAt": "2026-05-15T11:00:00Z",
  "metadata": {
    "tenant": "org-id",
    "userId": "user-id"
  },
  "data": {
    "name": "Super Administrator",
    "description": "Enhanced admin role",
    "isActive": true
  }
}
```

#### RoleDeletedDomainEvent

**Published when**: Role is soft-deleted via `DELETE /api/role/{id}`

**Schema**:
```json
{
  "eventType": "RoleDeletedDomainEvent",
  "eventId": "event-guid",
  "aggregateId": "role-guid",
  "occurredAt": "2026-05-15T12:00:00Z",
  "metadata": {
    "tenant": "org-id",
    "userId": "user-id"
  },
  "data": {
    "name": "Administrator",
    "description": "Full system access",
    "isActive": false
  }
}
```

### Event Consumers

#### RBAC Microservice

Consumes role events to maintain role-permission mappings:

```csharp
public class RoleCreatedEventHandler : IConsumer<RoleCreatedDomainEvent>
{
    public async Task Consume(ConsumeContext<RoleCreatedDomainEvent> context)
    {
        var evt = context.Message;
        
        // Create role record in RBAC service
        await rbacRepository.CreateRoleAsync(new RbacRole
        {
            Id = evt.AggregateId,
            Name = evt.Name,
            Permissions = GetDefaultPermissions(evt.Name)
        });
    }
}
```

#### Analytics Service

Tracks role usage and changes:

```csharp
public class RoleEventHandler : 
    IConsumer<RoleCreatedDomainEvent>,
    IConsumer<RoleUpdatedDomainEvent>,
    IConsumer<RoleDeletedDomainEvent>
{
    public async Task Consume(ConsumeContext<RoleCreatedDomainEvent> context)
    {
        await analyticsService.RecordEvent("role_created", new
        {
            roleId = context.Message.AggregateId,
            roleName = context.Message.Name,
            tenant = context.Message.Metadata["tenant"]
        });
    }
}
```

### RabbitMQ Configuration

Events are published to the `domain-events` exchange:

```json
{
  "RabbitMQ": {
    "Enable": true,
    "Host": "rabbitmq.codedesignplus.svc.cluster.local",
    "Port": 5672,
    "Exchange": "domain-events",
    "RoutingKeyPattern": "roles.{eventType}.{tenantId}"
  }
}
```

**Routing Keys**:
- `roles.RoleCreatedDomainEvent.org-a-id`
- `roles.RoleUpdatedDomainEvent.org-a-id`
- `roles.RoleDeletedDomainEvent.org-a-id`

### Testing Events Locally

```bash
# 1. Open RabbitMQ Management UI
open http://localhost:15672

# 2. Navigate to Exchanges → domain-events
# 3. Check Bindings to see consumer queues
# 4. Navigate to Queues → Select queue → Get Messages

# Or use CLI
rabbitmqadmin get queue=rbac-service-queue count=10
```

## 🐳 Docker Support

### Build Docker Image

```bash
# Build REST API image
docker build -t ms-roles-rest . -f src/entrypoints/CodeDesignPlus.Net.Microservice.Roles.Rest/Dockerfile

# Run container
docker run -d \
  -p 5000:5000 \
  --network=backend \
  -e ASPNETCORE_ENVIRONMENT=Docker \
  -e Vault__Address=http://vault:8200 \
  -e Vault__Token=root \
  --name ms-roles-rest \
  ms-roles-rest
```

### Docker Compose

```yaml
version: '3.8'

services:
  ms-roles-rest:
    image: ms-roles-rest
    build:
      context: .
      dockerfile: src/entrypoints/CodeDesignPlus.Net.Microservice.Roles.Rest/Dockerfile
    ports:
      - "5000:5000"
    environment:
      ASPNETCORE_ENVIRONMENT: Docker
      Vault__Address: http://vault:8200
      Vault__Token: root
    networks:
      - backend
    depends_on:
      - mongodb
      - redis
      - rabbitmq
      - vault

networks:
  backend:
    external: true
```

### Kubernetes Deployment

Helm charts are located in `charts/ms-roles-rest/`:

```bash
# Install in staging
helm upgrade --install ms-roles-rest ./charts/ms-roles-rest \
  -f ./charts/ms-roles-rest/Staging.yaml \
  --namespace codedesignplus \
  --create-namespace

# Install in production
helm upgrade --install ms-roles-rest ./charts/ms-roles-rest \
  -f ./charts/ms-roles-rest/Production.yaml \
  --namespace codedesignplus

# Check deployment
kubectl get pods -n codedesignplus | grep ms-roles
kubectl logs -n codedesignplus ms-roles-rest-xxx
```

**Chart Configuration** (`Staging.yaml`):
```yaml
replicaCount: 2
image:
  repository: ghcr.io/codedesignplus/ms-roles-rest
  tag: latest
resources:
  limits:
    cpu: 500m
    memory: 512Mi
  requests:
    cpu: 250m
    memory: 256Mi
env:
  - name: ASPNETCORE_ENVIRONMENT
    value: Staging
  - name: Vault__Address
    value: http://vault.codedesignplus.svc.cluster.local:8200
```

## ❓ FAQ

### General Questions

**Q: What is the difference between deactivating and deleting a role?**

A: Deactivating (`isActive = false`) temporarily disables a role without losing data. Users with deactivated roles lose access, but the role can be reactivated. Deleting (`DELETE /api/role/{id}`) performs a soft delete (`isDeleted = true`), which is permanent from the API perspective but preserves audit trails.

**Q: Can I permanently delete a role from the database?**

A: The API only supports soft deletes. Hard deletes must be performed directly in MongoDB if absolutely necessary. Soft deletes preserve audit trails and prevent data integrity issues.

**Q: How do I handle role name conflicts across tenants?**

A: Role names are NOT unique globally. Tenants can have roles with the same name. Uniqueness is enforced per tenant via the combination of `(name, tenant)`.

**Q: Can a role exist without permissions?**

A: Yes. Roles are created without permissions initially. Permissions are assigned in the RBAC microservice after the role is created.

### Integration Questions

**Q: How do I assign a role to a user?**

A: Role assignment is handled by the User microservice, not the Roles microservice. Use the User API:
```bash
POST /api/user/{userId}/roles
{
  "roleIds": ["admin-role-id", "editor-role-id"]
}
```

**Q: How do I check if a user has a specific role?**

A: Query the User microservice or RBAC microservice:
```bash
GET /api/user/{userId}/roles
# Returns: ["admin-role-id", "editor-role-id"]

GET /api/rbac/validate?userId={userId}&resource=roles&action=create
# Returns: { "hasPermission": true }
```

**Q: What happens to users when a role is deleted?**

A: The Roles microservice publishes a `RoleDeletedDomainEvent`. The User microservice should consume this event and:
1. Remove the role from all users
2. Optionally assign a default role
3. Notify affected users

### Technical Questions

**Q: How do I add custom fields to roles?**

A: Extend the `RoleAggregate` and related DTOs:

```csharp
// 1. Add property to aggregate
public class RoleAggregate : AggregateRootBase
{
    public string CustomField { get; private set; }
    
    public void SetCustomField(string value)
    {
        this.CustomField = value;
    }
}

// 2. Update DTOs
public class CreateRoleDto
{
    public string CustomField { get; set; }
}

// 3. Update commands and handlers
```

**Q: How do I enable RBAC permission checks?**

A: Configure in `appsettings.json`:
```json
{
  "Security": {
    "ValidateRbac": true,
    "ServerRbac": "http://ms-rbac:5001"
  }
}
```

**Q: Can I use this microservice without the RBAC microservice?**

A: Yes. Set `ValidateRbac = false` in configuration. The microservice will function as a standalone role management system without permission validation.

**Q: How do I migrate from another role system?**

A: Use bulk import via direct MongoDB insertion or create a migration script:

```csharp
foreach (var oldRole in legacyRoles)
{
    var command = new CreateRoleCommand(
        Guid.NewGuid(),
        oldRole.Name,
        oldRole.Description
    );
    await mediator.Send(command);
}
```

## 🧰 Tools & Scripts

The repository includes several utility scripts in the `tools/` directory:

### Update NuGet Packages

```bash
cd tools/update-packages
./update-packages.sh
```

Updates all CodeDesignPlus.Net.* packages to the latest versions.

### Upgrade .NET Version

```bash
cd tools/upgrade-dotnet
./upgrade-assistant.sh
```

Upgrades all `.csproj` files to a newer .NET version (e.g., .NET 9 → .NET 10).

### Configure Vault

```bash
cd tools/vault
./config-vault.sh
```

Seeds Vault with development secrets:
- MongoDB credentials: `user` / `pass`
- RabbitMQ credentials: `user` / `pass`

### SonarQube Analysis

```bash
cd tools/sonarqube
./sonar.sh
```

Runs SonarQube code analysis. Update `sonar.sh` with your SonarQube server URL and token.

### Convert Line Endings

```bash
cd tools
./convert-crlf-to-lf.sh
```

Converts all `.sh` files from CRLF to LF (required for Unix/Linux environments).

## 📊 Metrics & Monitoring

### OpenTelemetry

The microservice exports metrics and traces to OpenTelemetry:

```json
{
  "Observability": {
    "Enable": true,
    "ServerOtel": "http://localhost:4317",
    "Trace": {
      "Enable": true,
      "AspNetCore": true,
      "CodeDesignPlusSdk": true,
      "Redis": true,
      "RabbitMQ": true
    },
    "Metrics": {
      "Enable": true,
      "AspNetCore": true
    }
  }
}
```

### Key Metrics

- `http_server_request_duration` - Request latency
- `http_server_requests_total` - Request count
- `mongodb_commands_total` - MongoDB operations
- `rabbitmq_published_total` - Events published
- `redis_cache_hits_total` - Cache hit rate

### Health Checks

```bash
# Liveness (k8s liveness probe)
GET /health/live

# Readiness (k8s readiness probe)
GET /health/ready
```

### Logging

Structured logging with Serilog:

```json
{
  "Logger": {
    "Enable": true,
    "Level": "Information",
    "OTelEndpoint": "http://localhost:4317"
  }
}
```

Logs include:
- Request/response details
- Command/query execution
- Domain events published
- Error stack traces
- Correlation IDs for distributed tracing

## 🗺️ Roadmap

Planned features for future releases:

- [ ] **Hierarchical Roles**: Support parent-child role relationships
- [ ] **Role Templates**: Predefined role templates for common scenarios
- [ ] **Role Groups**: Group multiple roles into logical sets
- [ ] **Permission Inheritance**: Roles inherit permissions from parent roles
- [ ] **Role Expiration**: Time-limited role assignments
- [ ] **Role Approval Workflow**: Require approval before role activation
- [ ] **Bulk Operations**: Import/export roles via CSV or JSON
- [ ] **Role Cloning**: Duplicate existing roles with modifications
- [ ] **gRPC Entrypoint**: gRPC API for service-to-service communication
- [ ] **AsyncWorker Entrypoint**: Background job processing for bulk operations
- [ ] **GraphQL Support**: GraphQL API for flexible queries
- [ ] **Real-time Events**: WebSocket support for role change notifications

## 🤝 Contributing

Please read our [Contributing Guide](CONTRIBUTING.md) for details on our code of conduct and development process.

### Development Workflow

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Coding Standards

- Follow Clean Architecture principles
- Write unit tests for all business logic
- Use meaningful commit messages
- Update documentation for API changes
- Run `dotnet format` before committing

## 📄 License

This project is licensed under the GNU Lesser General Public License v3.0 - see the [LICENSE.md](LICENSE.md) file for details.

## 📞 Support

- **Documentation**: [https://codedesignplus.github.io/](https://codedesignplus.github.io/)
- **Issues**: [GitHub Issues](https://github.com/codedesignplus/CodeDesignPlus.Net.Microservice.Roles/issues)
- **Email**: support@codedesignplus.com
- **Slack**: [CodeDesignPlus Community](https://codedesignplus.slack.com)

---

**Built with ❤️ by CodeDesignPlus**
