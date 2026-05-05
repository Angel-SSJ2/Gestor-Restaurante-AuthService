using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Order>> GetByRestaurantIdAsync(Guid restaurantId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
    }
}