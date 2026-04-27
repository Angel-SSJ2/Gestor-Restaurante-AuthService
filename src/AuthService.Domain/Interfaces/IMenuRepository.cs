using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> GetByRestaurantIdAsync(Guid restaurantId);
        Task<Menu?> GetByIdAsync(Guid id);
        Task AddAsync(Menu menu);
        Task UpdateAsync(Menu menu);
        Task DeleteAsync(Guid id);
    }
}