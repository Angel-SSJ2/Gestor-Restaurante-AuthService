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

### Cración del Contenedor de Docker
```bash
docker run --name pg-auth-service \
  -e POSTGRES_DB=auth_restaurant_db \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=admin123 \
  -p 5433:5432 \
  -d postgres:latest
```

### 1. Clonar el repositorio
```bash
git clone [https://github.com/Angel-SSJ2/Gestor-Restaurante-AuthService.git](https://github.com/Angel-SSJ2/Gestor-Restaurante-AuthService.git)
cd Gestor-Restaurante-AuthService
```

### 2. Restaurar dependencias y CompilarBashdotnet restore
```bash
dotnet build
```
### 3. Correr el Proyecto
```bash
dotnet run
```

Una vez en ejecución, accede a Swagger en:
👉 http://localhost:5000/swagger

## 📋 Módulos del Sistema

| Módulo | Funcionalidad |
| :--- | :--- |
| **🛡️ AuthService** | Manejo de identidad, roles y tokens JWT. |
| **🏠 Restaurants** | Gestión de sucursales, horarios y datos generales. |
| **📅 Reservations** | Control de mesas, disponibilidad y fechas. |
| **🎉 Events** | Organización de noches temáticas y promociones. |
| **⭐ Reviews** | Sistema de feedback, comentarios y ratings. |
| **🍽️ Menus** | Administración de platillos, precios y categorías. |
