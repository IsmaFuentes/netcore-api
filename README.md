
# Web-api NET10 / JWT / EF


Plantilla de proyecto ASP.NET CORE Api con autenticación JWT 

```
API
└───Controllers
│   │   AuthController
│   │   UserController
|   |   HomeController
│   
└───Exceptions
|   |   ....
|
└───Middleware
│   │   ErrorHandlingMiddleware
|
└───Services
│   │   AuthService
│   │   UserService
|
| Program

Contracts
└───DTO
    | ....

Data
└───Entities
│   │   User
|
| Context
```

Endpoints

| URL            | HTTP   | Param                              | Result                                                   | Role                 |
|----------------|--------|------------------------------------|----------------------------------------------------------|----------------------|
| api/auth/login | POST   | [FromBody] DTO.AuthRequestDto      | IActionResult<DTO.AuthResponseDto> (JWT) \| Unauthorized | All                  |
| api/user       | GET    | [FromQuery] page \| pageSize       | IActionResult<DTO.PaginationResultDto>                   | User,Moderator,Admin |
| api/user/id    | GET    | id                                 | IActionResult<DTO.UserDTO> \| NotFound                   | Admin                |
| api/user       | POST   | [FromBody] DTO.UserRegistrationDto | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user       | PUT    | [FromBody] DTO.UserDto             | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user/id    | DELETE | id                                 | NoContent                                                | Admin                |