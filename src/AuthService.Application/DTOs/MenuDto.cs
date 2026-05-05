namespace AuthService.Application.DTOs
{
    public record CreateMenuDto(string Name, string? Description, Guid RestaurantId);
    
    public record PutMenuDto(string Name, string? Description);

    public record UpdateMenuDto(string? Name = null, string? Description = null);
}