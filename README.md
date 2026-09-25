# 🍺 MiApisBeer - Sistema Avanzado de Gestión e Inventario Cervecero

¡Bienvenido! Este es el backend de una aplicación empresarial para la gestión, distribución e inventario de cervezas, marcas y proveedores, desarrollado bajo una arquitectura robusta con **.NET 8 Web API** y **SQL Server**. El proyecto implementa altos estándares de seguridad corporativa, optimización de consultas y desacoplamiento de código.

## 🚀 Características Principales y Arquitectura

El sistema ha sido diseñado siguiendo las mejores prácticas de la industria y patrones de diseño modernos:

*   **Arquitectura Desacoplada (Repository Pattern):** Separación total de la lógica de negocio y las consultas físicas mediante contratos (`IBeerRepository`, `IBrandRepository`, `IProveedorRepository`). Esto permite cambiar de base de datos (ej. migrar a MongoDB) modificando una sola línea en la inyección de dependencias.
*   **Seguridad y Control de Accesos (RBAC):** Sistema de seguridad basado en roles corporativos (`Admin` y `Employee`). Los tokens JWT viajan firmados criptográficamente con los *Claims* del usuario, permitiendo accesos granulares (ej. Lectura libre para el público, creación para empleados y destrucción/edición exclusiva para el Administrador).
*   **Integridad Referencial Blindada (Anti-Crash):** Implementación de la regla de base de datos `ON DELETE SET NULL`. Si un proveedor o marca se elimina, el historial de stock no se destruye; el sistema desvincula el registro y el C# está protegido contra pantallas negras usando operadores de coalescencia nula (`?? 0`).
*   **Data Transfer Objects (DTOs):** Aislamiento completo de los modelos de Entity Framework a través de DTOs independientes para inserciones, actualizaciones y lecturas, optimizando el ancho de banda del JSON para el Frontend.
*   **Cifrado de Credenciales:** Hasheo criptográfico unidireccional de contraseñas de alta seguridad utilizando la librería **BCrypt.Net**.
*   **Documentación Automatizada con Soporte JWT:** Interfaz interactiva configurada en **Swagger UI** potenciada con la inyección nativa del botón de autorización Bearer, permitiendo pruebas autenticadas directo desde la web.

## 📁 Estructura del Modelo de Datos (Jerarquía Relacional)

La base de datos maneja una estructura de dependencias limpia y de escala comercial:
1.  **Proveedores (Suppliers):** Empresas mayoristas que nos surten de mercadería (ej. Backus).
2.  **Marcas (Brands):** Catálogo amarrado directamente a un Proveedor (Una marca pertenece a un proveedor).
3.  **Cervezas (Beers):** Productos finales indexados a una Marca (Una marca tiene muchas cervezas).
4.  **Usuarios (Users):** Registro centralizado de credenciales con asignación automática del rol por defecto `Employee`.

## 🛠️ Tecnologías y Librerías Utilizadas

*   **Lenguaje:** C# / .NET 8.0 (ASP.NET Core Web API)
*   **ORM:** Entity Framework Core (Enfoque Database First con LINQ avanzado)
*   **Base de Datos:** SQL Server
*   **Seguridad:** Microsoft.AspNetCore.Authentication.JwtBearer
*   **Criptografía:** BCrypt.Net-Next
*   **API Testing:** Postman (Colección automatizada con scripts de captura de token)

## 🔧 Instrucciones para Ejecución Local

### 1. Clonar el repositorio
```bash
git clone https://github.com
cd MiApisBeer
```

### 2. Configurar la Base de Datos (SQL Server)
Crea una base de datos llamada `Pub` (o el nombre de tu preferencia) y ejecuta el siguiente script maestro para levantar las tablas físicas y relaciones:

```sql
-- Tabla de Usuarios y Seguridad
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(MAX) NOT NULL,
    Role VARCHAR(20) NOT NULL DEFAULT 'Employee'
);

-- Tabla de Proveedores
CREATE TABLE Proveedores (
    ProveedoresId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    Ruc VARCHAR(11) NOT NULL,
    Telefono VARCHAR(20) NOT NULL
);

-- Tabla de Marcas
CREATE TABLE Brand (
    BrandId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    ProveedoresId INT NULL,
    CONSTRAINT FK_Brand_Proveedores FOREIGN KEY (ProveedoresId) REFERENCES Proveedores(ProveedoresId) ON DELETE SET NULL
);

-- Tabla de Cervezas
CREATE TABLE Beeer (
    BeerId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    BrandId INT NULL,
    CONSTRAINT FK_Beeer_Brand FOREIGN KEY (BrandId) REFERENCES Brand(BrandId) ON DELETE SET NULL
);
```

### 3. Configurar el Entorno (`appsettings.json`)
Asegúrate de colocar tu cadena de conexión local en la sección `ConnectionStrings` y de definir una frase secreta segura para la firma de tokens:
```json
{
  "ConnectionStrings": {
    "PubContext": "Server=TU_SERVIDOR;Database=Pub;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "UNA_CLAVE_SUPER_SECRETA_DE_MINIMO_32_CARACTERES"
  }
}
```

### 4. Ejecutar el Proyecto
Abre la solución `MiApisBeer.sln` en tu Visual Studio, espera que restaure los paquetes NuGet y presiona el botón **Play**. Swagger se encenderá automáticamente en tu navegador.

## 🚀 Pruebas y Control de Calidad (API Testing)

El proyecto incluye un entorno de pruebas profesional configurado en **Postman** estructurado de la siguiente forma:
*   **Subcarpetas Modulares:** Peticiones organizadas de forma limpia por CRUDs (`Cervezas`, `Marcas`, `Proveedores`, `Auth`).
*   **Automatización de Tokens:** El endpoint de *Iniciar Sesión* incluye un script en la pestaña `Tests` que captura el Token JWT de la respuesta y lo almacena de forma global. 
*   **Herencia de Autorización:** La colección principal hereda dinámicamente la variable `{{miTokenJwt}}` como un *Bearer Token*, permitiendo ejecutar peticiones protegidas sin necesidad de copiar y pegar el pase de seguridad manualmente.
