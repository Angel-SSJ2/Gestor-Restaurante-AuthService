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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync() => 
            await _context.Orders.Include(o => o.Customer).Include(o => o.Restaurant).ToListAsync();

        public async Task<Order?> GetByIdAsync(Guid id) => 
            await _context.Orders.Include(o => o.Customer).Include(o => o.Restaurant).FirstOrDefaultAsync(o => o.Id == id);

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId) =>
            await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();

        public async Task<IEnumerable<Order>> GetByRestaurantIdAsync(Guid restaurantId) =>
            await _context.Orders.Where(o => o.RestaurantId == restaurantId).ToListAsync();

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}