using AuthService.Domain.Entities;
using System;

namespace AuthService.Application.DTOs
{
    public record CreateTableDto(Guid RestaurantId, int TableNumber, int Capacity);
    
    public record PutTableDto(int TableNumber, int Capacity, TableStatus Status);

    public record UpdateTableStatusDto(TableStatus Status);
}