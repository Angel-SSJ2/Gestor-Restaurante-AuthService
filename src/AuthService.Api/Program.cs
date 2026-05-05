using AuthService.Api.Extensions;
using AuthService.Api.Middlewares;
using AuthService.Api.ModelBinders;
using AuthService.Persistence.Data;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;
using Serilog;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN INICIAL ---

// FIX: Bypass SSL para servicios externos si es necesario
System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

// Configuración de Serilog
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

// --- 2. REGISTRO DE SERVICIOS ---

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new FileDataModelBinderProvider());
})
.AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Métodos de extensión para modularizar la configuración
builder.Services.AddApiDocumentation();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddRateLimitingPolicies();
builder.Services.AddSecurityPolicies(builder.Configuration);
builder.Services.AddSecurityOptions();

builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger con soporte para XML Comments
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// --- 3. MIDDLEWARES (EL ORDEN IMPORTA) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Si quieres que Swagger cargue en la raíz, descomenta:
        // c.RoutePrefix = string.Empty;
    });
}

app.UseSerilogRequestLogging();

// CONFIGURACIÓN DE SEGURIDAD (Ajustada para permitir Swagger)
app.UseSecurityHeaders(policies => policies
    .AddDefaultSecurityHeaders()
    .RemoveServerHeader()
    .AddFrameOptionsSameOrigin() // Cambiado de Deny a SameOrigin para Swagger
    .AddXssProtectionBlock()
    .AddContentTypeOptionsNoSniff()
    .AddReferrerPolicyStrictOriginWhenCrossOrigin()
    .AddContentSecurityPolicy(cspBuilder =>
    {
        cspBuilder.AddDefaultSrc().Self();
        // Permitimos unsafe-inline para que Swagger UI pueda renderizar sus estilos y scripts
        cspBuilder.AddScriptSrc().Self().UnsafeInline();
        cspBuilder.AddStyleSrc().Self().UnsafeInline();
        cspBuilder.AddImgSrc().Self().Data();
        cspBuilder.AddFontSrc().Self().Data();
        // Permitir que el navegador haga fetch a la propia API
        cspBuilder.AddConnectSrc().Self(); 
        cspBuilder.AddFrameAncestors().None();
        cspBuilder.AddBaseUri().Self();
        cspBuilder.AddFormAction().Self();
    })
    .AddCustomHeader("Permissions-Policy", "geolocation=(), microphone=(), camera=()")
    .AddCustomHeader("Cache-Control", "no-store, no-cache, must-revalidate, private")
);

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("DefaultCorsPolicy");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- 4. ENDPOINTS DE SALUD (HEALTH CHECKS) ---

app.MapHealthChecks("/health/status"); // Endpoint estándar de .NET cambiado para evitar conflicto

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
    });
});

app.MapHealthChecks("/api/v1/health");

// --- 5. LOG DE INICIO ---

var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        var server = app.Services.GetRequiredService<Microsoft.AspNetCore.Hosting.Server.IServer>();
        var addressesFeature = server.Features.Get<IServerAddressesFeature>();
        var addresses = addressesFeature?.Addresses ?? app.Urls;

        if (addresses != null && addresses.Any())
        {
            foreach (var addr in addresses)
            {
                startupLogger.LogInformation("AuthService API is running at {Url}. Swagger: {Url}/swagger", addr, addr.TrimEnd('/'));
            }
        }
    }
    catch (Exception ex)
    {
        startupLogger.LogWarning(ex, "Failed to determine the listening addresses");
    }
});

// --- 6. INICIALIZACIÓN DE DB (MIGRACIONES Y SEED) ---

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Checking database connection...");
        
        // EnsureCreatedAsync crea la base de datos si no existe basándose en tu ApplicationDbContext
        await context.Database.EnsureCreatedAsync();

        logger.LogInformation("Database ready. Running seed data...");
        await DataSeeder.SeedAsync(context);

        logger.LogInformation("Database initialization completed successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database");
        // No lanzamos throw aquí para permitir que la app intente iniciar aunque falle la DB 
        // (útil para debuggear el frontend/swagger)
    }
}

app.Run();