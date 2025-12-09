# Web API .NET 10 / JWT / EF

**ASP.NET Core API** Entity Framework + JWT authentication
This template provides the basic structure to build RESTful APIs with support for authentication and authorization.

## Key aspects

- **JWT Authentication**: Users must be logged in to be able to access protected endpoints
- **User CRUD**: Basic operations for users, (Get/Create/Update/Delete)
- **Role based authorization**: Roles such as `Admin`, `User` or `Moderator` control the access to different endpoints.
- **Centralized error handling**: Exceptions are handled by a middleware that returns a standardized error response using the "ProblemDetails" structure.
- **Model State validations**: Implements the use of filters that validate required fields
- **DTO Mapping**: The information exchange is done exclusively with DTOs
- **Ensures SQL Server compatibility**: Breaking changes in EF are handled gracefully using the compat level required by the SQL engine in use

## Project structure (Layered design)

```
API
└───Controllers
│		└─── Filters
│		│		│....
│   │   AuthController
│   │   UserController
|   |   HomeController
│   
└───Exceptions
|   |   ....
│  
└───Middleware
│   │   ErrorHandlingMiddleware
│
└─── Mapping
│		│ UserMappingExtensions
│
└───Services
│		└─── Interfaces
│		|   | ....
│   │   AuthService
│   │   UserService
|
│   Program

Contracts
└───DTO
    | ....

Data
└───Entities
│   │   User
|
│ Context
```

## Endpoints

| URL            | HTTP   | Param                              | Result                                                   | Role                 |
|----------------|--------|------------------------------------|----------------------------------------------------------|----------------------|
| api/auth/login | POST   | [FromBody] DTO.AuthRequestDto      | IActionResult<DTO.AuthResponseDto> (JWT) \| Unauthorized | Anonymous						 |
| api/user       | GET    | [FromQuery] page \| pageSize       | IActionResult<DTO.PaginationResultDto>                   | User,Moderator,Admin |
| api/user/:id   | GET    | id                                 | IActionResult<DTO.UserDTO> \| NotFound                   | Admin                |
| api/user       | POST   | [FromBody] DTO.UserRegistrationDto | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user       | PUT    | [FromBody] DTO.UserDto             | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user/:id   | DELETE | id                                 | NoContent                                                | Admin                |

## Requirements

- .NET 10
- Microsoft SQL Server