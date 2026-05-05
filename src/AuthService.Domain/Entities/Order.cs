using System;

namespace AuthService.Domain.Entities
{
    public enum OrderStatus
    {
        Pending,
        Preparing,
        Ready,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? Notes { get; set; }

        public Guid CustomerId { get; set; }
        public User? Customer { get; set; }

        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}