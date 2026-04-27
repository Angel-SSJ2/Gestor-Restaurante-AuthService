using AuthService.Domain.Entities;
using System;

namespace AuthService.Application.DTOs
{
    public record CreateReservationDto(
        Guid RestaurantId, 
        DateTime ReservationDate, 
        int PeopleCount, 
        string? SpecialRequests
    );
    
    public record PutReservationDto(
        DateTime ReservationDate, 
        int PeopleCount, 
        ReservationStatus Status, 
        string? SpecialRequests
    );

    public record UpdateReservationStatusDto(ReservationStatus Status);
}