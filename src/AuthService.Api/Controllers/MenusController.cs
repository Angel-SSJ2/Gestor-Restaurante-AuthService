using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MenusController : ControllerBase
    {
        private readonly IMenuRepository _repository;

        public MenusController(IMenuRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
        {
            var menus = await _repository.GetByRestaurantIdAsync(restaurantId);
            return Ok(menus);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var menu = await _repository.GetByIdAsync(id);
            if (menu == null) return NotFound();
            return Ok(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMenuDto dto)
        {
            var menu = new Menu 
            { 
                Name = dto.Name, 
                Description = dto.Description, 
                RestaurantId = dto.RestaurantId 
            };
            await _repository.AddAsync(menu);
            return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Replace(Guid id, [FromBody] PutMenuDto dto)
        {
            var menu = await _repository.GetByIdAsync(id);
            if (menu == null) return NotFound();

            menu.Name = dto.Name;
            menu.Description = dto.Description;

            await _repository.UpdateAsync(menu);
            return Ok(menu);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMenuDto dto)
        {
            var menu = await _repository.GetByIdAsync(id);
            if (menu == null) return NotFound();

            if (dto.Name != null) menu.Name = dto.Name;
            if (dto.Description != null) menu.Description = dto.Description;

            await _repository.UpdateAsync(menu);
            return Ok(menu);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}