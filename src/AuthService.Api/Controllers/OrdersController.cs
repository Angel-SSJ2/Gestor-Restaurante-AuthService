using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;

        public OrdersController(IOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = await _repository.GetByCustomerIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var order = new Order 
            { 
                CustomerId = userId,
                RestaurantId = dto.RestaurantId,
                TotalAmount = dto.TotalAmount,
                Notes = dto.Notes,
                Status = OrderStatus.Pending
            };

            await _repository.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // PUT: Reemplazo total de la orden
        [HttpPut("{id}")]
        public async Task<IActionResult> Replace(Guid id, [FromBody] PutOrderDto dto)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) return NotFound();

            order.TotalAmount = dto.TotalAmount;
            order.Status = dto.Status;
            order.Notes = dto.Notes;

            await _repository.UpdateAsync(order);
            return Ok(order);
        }

        // PATCH: Solo actualizar el estado de la orden (Muy usado por repartidores/restaurantes)
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) return NotFound();

            order.Status = dto.Status;
            await _repository.UpdateAsync(order);
            return Ok(order);
        }
    }
}