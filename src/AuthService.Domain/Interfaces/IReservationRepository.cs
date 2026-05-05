using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(Guid id);
        Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(Guid restaurantId);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Guid id);
    }
}