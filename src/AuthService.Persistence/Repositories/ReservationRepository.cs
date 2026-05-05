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
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync() => 
            await _context.Reservations.Include(r => r.Customer).Include(r => r.Restaurant).ToListAsync();

        public async Task<Reservation?> GetByIdAsync(Guid id) => 
            await _context.Reservations.Include(r => r.Customer).Include(r => r.Restaurant)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId) =>
            await _context.Reservations.Where(r => r.CustomerId == customerId).ToListAsync();

        public async Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(Guid restaurantId) =>
            await _context.Reservations.Where(r => r.RestaurantId == restaurantId).ToListAsync();

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservation = await GetByIdAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}