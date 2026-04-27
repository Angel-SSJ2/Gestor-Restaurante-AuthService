using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Persistence.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly ApplicationDbContext _context;

        public TableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Table>> GetByRestaurantIdAsync(Guid restaurantId) =>
            await _context.Tables
                .Where(t => t.RestaurantId == restaurantId && t.IsActive)
                .OrderBy(t => t.TableNumber)
                .ToListAsync();

        public async Task<Table?> GetByIdAsync(Guid id) =>
            await _context.Tables.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

        public async Task AddAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var table = await GetByIdAsync(id);
            if (table != null)
            {
                table.IsActive = false; // Borrado lógico
                await _context.SaveChangesAsync();
            }
        }
    }
}