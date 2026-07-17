# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de solución y proyectos primero para optimizar la caché de capas de Docker
COPY AuthService.sln ./
COPY src/AuthService.Api/AuthService.Api.csproj src/AuthService.Api/
COPY src/AuthService.Application/AuthService.Application.csproj src/AuthService.Application/
COPY src/AuthService.Domain/AuthService.Domain.csproj src/AuthService.Domain/
COPY src/AuthService.Persistence/AuthService.Persistence.csproj src/AuthService.Persistence/

# Restaurar paquetes NuGet
RUN dotnet restore

# Copiar el resto del código y compilar la API
COPY src/ src/
RUN dotnet publish src/AuthService.Api/AuthService.Api.csproj -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Fix for .NET 8 permissions (Serilog and DataProtection need write access)
RUN chown -R app:app /app
USER app

# Configurar el puerto de escucha para que ASP.NET Core enlace con el puerto expuesto
ENV ASPNETCORE_URLS=http://+:5156
EXPOSE 5156

ENTRYPOINT ["dotnet", "AuthService.Api.dll"]