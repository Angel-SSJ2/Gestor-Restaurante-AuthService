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
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetByRestaurantIdAsync(Guid restaurantId) =>
            await _context.Events
                .Where(e => e.RestaurantId == restaurantId && e.IsActive)
                .OrderBy(e => e.StartDate)
                .ToListAsync();

        public async Task<Event?> GetByIdAsync(Guid id) =>
            await _context.Events.FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

        public async Task AddAsync(Event @event)
        {
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event @event)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var @event = await GetByIdAsync(id);
            if (@event != null)
            {
                @event.IsActive = false; // Borrado lógico
                await _context.SaveChangesAsync();
            }
        }
    }
}