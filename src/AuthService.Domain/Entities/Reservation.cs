using System;

namespace AuthService.Domain.Entities
{
    public enum ReservationStatus
    {
        Confirmed,
        Pending,
        Cancelled,
        Completed
    }

    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime ReservationDate { get; set; } // Fecha y hora de la reserva
        public int PeopleCount { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public string? SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relación con el Cliente (Usuario)
        public Guid CustomerId { get; set; }
        public User? Customer { get; set; }

        // Relación con el Restaurante
        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}