# Web API .NET 10 / JWT / EF

Este es un proyecto de ejemplo basado en **ASP.NET Core Web API** con autenticación **JWT** y **Entity Framework (EF)**. Esta plantilla proporciona una estructura básica para construir APIs RESTful con soporte para autenticación y autorización.

## Estructura del Proyecto


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

## Características

- **Autenticación JWT**: Los usuarios pueden iniciar sesión para obtener un **JWT** que se usará para autenticación en todos los endpoints protegidos.
- **CRUD de Usuarios**: Los usuarios pueden ser creados, actualizados, eliminados y listados.
- **Autorización basada en Roles**: Los roles de usuario como `Admin`, `User`, `Moderator` controlan el acceso a los diferentes endpoints.
- **Manejo de errores**: Middleware que captura y devuelve errores de manera uniforme.

## Endpoints

| URL            | HTTP   | Param                              | Result                                                   | Role                 |
|----------------|--------|------------------------------------|----------------------------------------------------------|----------------------|
| api/auth/login | POST   | [FromBody] DTO.AuthRequestDto      | IActionResult<DTO.AuthResponseDto> (JWT) \| Unauthorized | All                  |
| api/user       | GET    | [FromQuery] page \| pageSize       | IActionResult<DTO.PaginationResultDto>                   | User,Moderator,Admin |
| api/user/id    | GET    | id                                 | IActionResult<DTO.UserDTO> \| NotFound                   | Admin                |
| api/user       | POST   | [FromBody] DTO.UserRegistrationDto | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user       | PUT    | [FromBody] DTO.UserDto             | IActionResult<DTO.UserDTO>                               | Admin                |
| api/user/id    | DELETE | id                                 | NoContent                                                | Admin                |

## Manejo de errores

La API utiliza un middleware de manejo de errores para capturar y devolver errores en un formato uniforme. Si se lanza una excepción como NotFoundException, UserAlreadyExistsException, o cualquier otro tipo de error, el middleware devolverá un mensaje adecuado con el código de estado correspondiente (por ejemplo, 404 Not Found o 400 Bad Request).

## Requisitos

- **.NET 6.0 o superior**
- **SQL Server** (o cualquier otro proveedor de base de datos compatible con Entity Framework)
- **Postman o cualquier herramienta para probar APIs**

## Instalación

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu_usuario/tu_repositorio.git
```

### 2. Configurar la base de datos

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NombreBaseDeDatos;User Id=usuario;Password=contraseña;"
  }
}
```

### 3. Restaurar dependencias

```bash
dotnet restore
```

### 4. Ejecutar el proyecto

```bash
dotnet run
```