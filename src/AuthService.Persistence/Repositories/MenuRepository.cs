using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menu>> GetByRestaurantIdAsync(Guid restaurantId) =>
            await _context.Menus.Where(m => m.RestaurantId == restaurantId && m.IsActive).ToListAsync();

        public async Task<Menu?> GetByIdAsync(Guid id) =>
            await _context.Menus.FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

        public async Task AddAsync(Menu menu)
        {
            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var menu = await GetByIdAsync(id);
            if (menu != null)
            {
                menu.IsActive = false; // Borrado lógico
                await _context.SaveChangesAsync();
            }
        }
    }
}