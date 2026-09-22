# 🍺 MiApisBeer - Backend .NET Core

¡Bienvenido! Este es el backend de una aplicación profesional para la gestión de cervezas y marcas, desarrollado con **.NET 8 Web API** y conectado a **SQL Server**. El proyecto implementa una arquitectura desacoplada, segura y escalable ideal para entornos de producción.

## 🚀 Características Principales y Arquitectura

El proyecto ha sido migrado de un controlador tradicional hacia una estructura limpia utilizando las mejores prácticas de la industria:

*   **Patrón Repositorio e Interfaces:** Separación total de la lógica de negocio y el acceso a datos. Los controladores interactúan únicamente con interfaces (`IBeerRepository`, `IBrandRepository`), garantizando un código testeable y modular.
*   **Data Transfer Objects (DTOs):** Implementación de DTOs específicos para el ingreso y salida de datos, protegiendo las entidades de la base de datos y optimizando las respuestas JSON para el Frontend.
*   **Seguridad con JWT (JSON Web Tokens):** Endpoints completamente blindados mediante la etiqueta `[Authorize]`. Implementación del flujo de autenticación (*Register / Login*) utilizando tokens de portador (*Bearer Tokens*).
*   **Cifrado de Contraseñas:** Seguridad avanzada en la base de datos mediante el hasheo criptográfico de contraseñas con la librería **BCrypt.Net**.
*   **Documentación Automatizada:** Interfaz gráfica interactiva configurada en **Swagger UI**, incluyendo soporte visual para pruebas con autenticación Bearer JWT.

## 🛠️ Tecnologías Utilizadas

*   **C#** / **.NET 8.0** (ASP.NET Core Web API)
*   **Entity Framework Core** (Enfoque Database First / LINQ)
*   **SQL Server** (Base de datos relacional)
*   **BCrypt.Net-Next** (Cifrado de credenciales)
*   **Microsoft.AspNetCore.Authentication.JwtBearer** (Seguridad de endpoints)

## 🔧 Instrucciones para Ejecución Local

Para clonar y correr este proyecto en tu entorno local, sigue estos pasos:

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com
   ```

2. **Configurar la base de datos:**
   Crea una base de datos en tu SQL Server local y ejecuta el siguiente script para la tabla de seguridad:
   ```sql
   CREATE TABLE Users (
       UserId INT IDENTITY(1,1) PRIMARY KEY,
       Email VARCHAR(100) NOT NULL,
       PasswordHash VARCHAR(MAX) NOT NULL
   );
   ```

3. **Configurar el archivo appsettings.json:**
   Asegúrate de colocar tu cadena de conexión local (`ConnectionStrings:PubContext`) y definir una frase secreta segura de mínimo 32 caracteres en la sección `Jwt:Key`.

4. **Ejecutar el proyecto:**
   Abre el archivo `MiApisBeer.sln` en Visual Studio y presiona el botón **Play**. ¡La interfaz de Swagger se abrirá automáticamente en tu navegador!
