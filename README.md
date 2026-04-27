# 🍽️ Gestor de Restaurantes - AuthService

Sistema backend robusto para la gestión integral de restaurantes, desarrollado con **.NET 8** bajo una **Arquitectura de Capas**. Este servicio centraliza la autenticación, gestión de reservas, eventos, menús y reseñas.

---

## 🏗️ Arquitectura del Proyecto

El proyecto está organizado en 4 capas para garantizar la escalabilidad y el mantenimiento del código:

* **`src/AuthService.Domain`**: 🧩 Contiene las entidades de negocio (`Restaurant`, `Event`, `Order`, `Review`, etc.) e interfaces.
* **`src/AuthService.Application`**: ⚙️ Lógica de aplicación, mapeo de DTOs y validaciones.
* **`src/AuthService.Persistence`**: 💾 Acceso a datos, `DbContext` de Entity Framework y repositorios.
* **`src/AuthService.Api`**: 🌐 Controladores REST, configuración de inyección de dependencias y JWT.

---

## 🛠️ Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

## 🚀 Instalación y Construcción

Sigue estos pasos para levantar el proyecto localmente:

### 1. Clonar el repositorio
```bash
git clone [https://github.com/Angel-SSJ2/Gestor-Restaurante-AuthService.git](https://github.com/Angel-SSJ2/Gestor-Restaurante-AuthService.git)
cd Gestor-Restaurante-AuthService
```

```bash
2. Restaurar dependencias y CompilarBashdotnet restore
dotnet build
```

```bash
3. Actualizar Base de Datos (Migrations)Bashdotnet ef database update --project src/AuthService.Persistence --startup-project src/AuthService.Api
🐳 DockerizaciónEl proyecto incluye configuración para ejecutarse en contenedores Docker de manera eficiente.Construir la imagen localDesde la raíz del proyecto, ejecuta el siguiente comando:Bashdocker build -t auth-service-api -f src/AuthService.Api/Dockerfile .
Levantar el contenedorBashdocker run -d -p 8080:8080 --name gestor-restaurante auth-service-api
```


🏃 Cómo Correr el ProyectoUsando la CLI de .NETBashdotnet run --project src/AuthService.Api/AuthService.Api.csproj
Una vez en ejecución, puedes acceder a la documentación interactiva (Swagger) en:👉 http://localhost:5000/swagger📋 Módulos del SistemaMóduloFuncionalidad🛡️ AuthServiceManejo de identidad, roles y tokens JWT.🏠 RestaurantsGestión de sucursales, horarios y datos generales.📅 ReservationsControl de mesas, disponibilidad y fechas.🎉 EventsOrganización de noches temáticas y promociones.⭐ ReviewsSistema de feedback, comentarios y ratings.🍽️ MenusAdministración de platillos, precios y categorías.
