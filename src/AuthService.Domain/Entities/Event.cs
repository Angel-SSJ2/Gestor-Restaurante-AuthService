using System;

namespace AuthService.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; } // Puede ser "Terraza", "Salón Principal", etc.
        public bool IsActive { get; set; } = true;

        // Relación: Un Evento pertenece a un Restaurante
        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}