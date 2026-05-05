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
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _repository;

        public RestaurantsController(IRestaurantRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyRestaurants()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null) return Unauthorized();

            var restaurants = await _repository.GetByOwnerIdAsync(Guid.Parse(userIdStr));
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();
            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null) return Unauthorized();

            var restaurant = new Restaurant 
            { 
                Name = dto.Name, 
                Address = dto.Address, 
                PhoneNumber = dto.PhoneNumber,
                OwnerId = Guid.Parse(userIdStr)
            };

            await _repository.AddAsync(restaurant);
            return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant);
        }

        // PUT: Reemplazo total
        [HttpPut("{id}")]
        public async Task<IActionResult> Replace(Guid id, [FromBody] PutRestaurantDto dto)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();

            restaurant.Name = dto.Name;
            restaurant.Address = dto.Address;
            restaurant.PhoneNumber = dto.PhoneNumber;

            await _repository.UpdateAsync(restaurant);
            return Ok(restaurant);
        }

        // PATCH: Actualización parcial
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRestaurantDto dto)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();

            if (dto.Name != null) restaurant.Name = dto.Name;
            if (dto.Address != null) restaurant.Address = dto.Address;
            if (dto.PhoneNumber != null) restaurant.PhoneNumber = dto.PhoneNumber;

            await _repository.UpdateAsync(restaurant);
            return Ok(restaurant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}