using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetByRestaurantIdAsync(Guid restaurantId);
        Task<Table?> GetByIdAsync(Guid id);
        Task AddAsync(Table table);
        Task UpdateAsync(Table table);
        Task DeleteAsync(Guid id);
    }
}