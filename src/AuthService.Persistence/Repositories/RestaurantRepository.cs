using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync() => 
            await _context.Restaurants.Where(r => r.IsActive).ToListAsync();

        public async Task<Restaurant?> GetByIdAsync(Guid id) => 
            await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

        public async Task<IEnumerable<Restaurant>> GetByOwnerIdAsync(Guid ownerId) =>
            await _context.Restaurants.Where(r => r.OwnerId == ownerId && r.IsActive).ToListAsync();

        public async Task AddAsync(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var restaurant = await GetByIdAsync(id);
            if (restaurant != null)
            {
                restaurant.IsActive = false; // Borrado lógico
                await _context.SaveChangesAsync();
            }
        }
    }
}