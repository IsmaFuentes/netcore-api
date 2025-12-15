# ASP.NET CORE Web API 

![.NET](https://img.shields.io/badge/.NET-10-blue)

Modern REST API built with **ASP.NET Core 10**, **Entity Framework Core**, and **JWT authentication**.  
Designed as a clean and reusable **template** for small/medium projects or for learning modern ASP.NET Core patterns.

## Requirements

- NET 10
- Microsoft SQL Server

## Getting Started

-  Clone the repository  
   `git clone https://github.com/....`

- Configure **appsettings.json**  
   Add your SQL Server connection string and JWT options:
   ```json
   {
     "ConnectionStrings": { "DefaultConnection": "YOUR-CONNECTION" },
     "Jwt": {
       "Issuer": "...",
       "Audience": "...",
       "SecretKey": "..."
     }
   }
   ```

- Run the project using **dotnet run** or with Visual Studio

## Features

- **JWT Authentication**  
  Secure login with token-based authentication.

- **User CRUD**  
  Create, update, delete, and list users.

- **Role-based Authorization**  
  Built-in roles: `Admin`, `Moderator`, `User`.

- **Centralized Error Handling**  
  Custom middleware returns standardized responses using **ProblemDetails**.

- **DTO-Only Communication**  
  No direct exposure of EF entities.

- **Automatic Model Validation**  
  Global validation filter for missing/invalid fields.

- **SQL Server Compatibility Level**  
  Prevents breaking changes between SQL server versions.

- **Auto Database Initialization (development mode)**  
  When running in `Development`, the API:
  - Creates the database automatically  
  - Seeds default users: **Admin**, **Moderator**, **User**

## Endpoints

| URL            | HTTP   | Param                              | Result                                                   | Role                 |
|----------------|--------|------------------------------------|----------------------------------------------------------|----------------------|
| api/auth/login | POST   | [FromBody] DTO.AuthRequestDto      | IActionResult<DTO.AuthResponseDto> (JWT) \| Unauthorized | Anonymous						 |
| api/user       | GET    | [FromQuery] page \| pageSize       | IActionResult<DTO.PaginationResultDto>                   | User,Moderator,Admin |
| api/user/:id   | GET    | id                                 | IActionResult<DTO.UserDTO> \| NotFound                   | Admin                |
| api/user       | POST   | [FromBody] DTO.UserRegistrationDto | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user       | PUT    | [FromBody] DTO.UserDto             | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user/:id   | DELETE | id                                 | NoContent                                                | Admin                |