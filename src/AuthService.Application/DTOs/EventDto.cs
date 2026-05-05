using System;

namespace AuthService.Application.DTOs
{
    public record CreateEventDto(
        Guid RestaurantId, 
        string Title, 
        string? Description, 
        DateTime StartDate, 
        DateTime EndDate, 
        string? Location
    );
    
    public record PutEventDto(
        string Title, 
        string? Description, 
        DateTime StartDate, 
        DateTime EndDate, 
        string? Location
    );

    public record UpdateEventDto(
        string? Title = null, 
        string? Description = null, 
        DateTime? StartDate = null, 
        DateTime? EndDate = null
    );
}