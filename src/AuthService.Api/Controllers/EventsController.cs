using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _repository;

        public EventsController(IEventRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("restaurant/{restaurantId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
        {
            var events = await _repository.GetByRestaurantIdAsync(restaurantId);
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var @event = await _repository.GetByIdAsync(id);
            if (@event == null) return NotFound();
            return Ok(@event);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
        {
            var @event = new Event 
            { 
                RestaurantId = dto.RestaurantId,
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Location = dto.Location
            };

            await _repository.AddAsync(@event);
            return CreatedAtAction(nameof(GetById), new { id = @event.Id }, @event);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Replace(Guid id, [FromBody] PutEventDto dto)
        {
            var @event = await _repository.GetByIdAsync(id);
            if (@event == null) return NotFound();

            @event.Title = dto.Title;
            @event.Description = dto.Description;
            @event.StartDate = dto.StartDate;
            @event.EndDate = dto.EndDate;
            @event.Location = dto.Location;

            await _repository.UpdateAsync(@event);
            return Ok(@event);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}