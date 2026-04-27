namespace AuthService.Application.DTOs
{
    // Para POST (Creación)
    public record CreateRestaurantDto(string Name, string Address, string? PhoneNumber);
    
    // Para PUT (Reemplazo total)
    public record PutRestaurantDto(string Name, string Address, string? PhoneNumber);

    // Para PATCH (Actualización parcial)
    public record UpdateRestaurantDto(
        string? Name = null, 
        string? Address = null, 
        string? PhoneNumber = null
    );
}