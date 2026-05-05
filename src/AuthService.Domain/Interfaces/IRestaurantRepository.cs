using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(Guid id);
        Task<IEnumerable<Restaurant>> GetByOwnerIdAsync(Guid ownerId);
        Task AddAsync(Restaurant restaurant);
        Task UpdateAsync(Restaurant restaurant);
        Task DeleteAsync(Guid id);
    }
}