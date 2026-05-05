using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetByRestaurantIdAsync(Guid restaurantId);
        Task<Event?> GetByIdAsync(Guid id);
        Task AddAsync(Event @event);
        Task UpdateAsync(Event @event);
        Task DeleteAsync(Guid id);
    }
}