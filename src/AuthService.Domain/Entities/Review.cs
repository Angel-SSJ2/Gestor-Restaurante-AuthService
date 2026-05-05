using System;

namespace AuthService.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Rating { get; set; } // Ejemplo: 1 a 5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relación con el Cliente (Usuario)
        public Guid CustomerId { get; set; }
        public User? Customer { get; set; }

        // Relación con el Restaurante
        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}