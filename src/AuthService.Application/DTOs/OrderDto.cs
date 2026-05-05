using AuthService.Domain.Entities;
using System;

namespace AuthService.Application.DTOs
{
    public record CreateOrderDto(Guid RestaurantId, decimal TotalAmount, string? Notes);
    
    public record PutOrderDto(decimal TotalAmount, OrderStatus Status, string? Notes);

    public record UpdateOrderStatusDto(OrderStatus Status);
}