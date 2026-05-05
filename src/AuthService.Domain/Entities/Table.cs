using System;

namespace AuthService.Domain.Entities
{
    public enum TableStatus
    {
        Available,
        Occupied,
        Reserved,
        Maintenance
    }

    public class Table
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; } = TableStatus.Available;
        public bool IsActive { get; set; } = true;

        // Relación: Una Mesa pertenece a un Restaurante
        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}