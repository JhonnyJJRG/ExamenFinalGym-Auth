🔐 MS-Auth - Microservicio de Autenticación e Identidad
Este microservicio es el punto de entrada de seguridad para el ecosistema GymMaster. Se encarga de la gestión centralizada de usuarios, el control de acceso basado en roles (RBAC) y la emisión de tokens de seguridad.

📝 Descripción
El MS-Auth implementa un sistema de identidad desacoplado que permite a los demás microservicios validar la autenticidad de las peticiones sin necesidad de consultar la base de datos de usuarios constantemente, utilizando el estándar JSON Web Token (JWT).

🛠️ Stack Tecnológico
Framework: .NET 8/9 (ASP.NET Core Web API)

ORM: Entity Framework Core

Base de Datos: SQL Server (Tablas: Users, Roles, UserRoles)

Seguridad: Microsoft.AspNetCore.Authentication.JwtBearer

⚙️ Configuración (appsettings.json)
Para que el servicio funcione correctamente, asegúrese de configurar el nodo de seguridad y la cadena de conexión:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GimnasioDB;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "MiClaveSuperSecretaParaElGimnasio2026",
    "Issuer": "GymBackend",
    "Audience": "GymUsers"
  }
}

📍 Endpoints Principales
Autenticación
POST /api/auth/login: Recibe credenciales (Email, Password) y devuelve un Token JWT junto con la fecha de expiración.

POST /api/auth/refresh: (Opcional) Renueva el token de acceso.

Gestión de Usuarios
GET /api/usuarios: Lista todos los usuarios registrados (Solo ADMIN).

POST /api/usuarios: Registra un nuevo usuario en el sistema.

GET /api/usuarios/{id}: Obtiene el detalle de un usuario específico.

Gestión de Roles
GET /api/roles: Lista los roles disponibles (ADMIN, ENTRENADOR, SOCIO).

🚀 Ejecución
Para levantar este servicio de forma independiente:
cd MS-Auth
dotnet restore
dotnet run

Puerto por defecto: http://localhost:5092

🛡️ Seguridad Implementada
Hashing de Contraseñas: Las contraseñas se almacenan mediante algoritmos de derivación de claves (no texto plano).

Claims: El token generado incluye el UserId, Email y el Role del usuario, permitiendo la autorización en los microservicios de Administración, Rutinas y Pagos.