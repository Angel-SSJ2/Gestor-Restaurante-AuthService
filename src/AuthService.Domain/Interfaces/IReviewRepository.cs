using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByRestaurantIdAsync(Guid restaurantId);
        Task<Review?> GetByIdAsync(Guid id);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Guid id);
    }
}