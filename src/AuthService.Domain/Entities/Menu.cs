using System;

namespace AuthService.Domain.Entities
{
    public class Menu
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relación: Un Menú pertenece a un Restaurante
        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}