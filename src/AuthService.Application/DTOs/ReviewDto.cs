using System;

namespace AuthService.Application.DTOs
{
    public record CreateReviewDto(Guid RestaurantId, int Rating, string? Comment);
    
    public record PutReviewDto(int Rating, string? Comment);

    public record UpdateReviewDto(int? Rating = null, string? Comment = null);
}