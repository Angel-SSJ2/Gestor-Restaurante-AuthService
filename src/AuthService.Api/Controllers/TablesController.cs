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
    public class TablesController : ControllerBase
    {
        private readonly ITableRepository _repository;

        public TablesController(ITableRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
        {
            var tables = await _repository.GetByRestaurantIdAsync(restaurantId);
            return Ok(tables);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var table = await _repository.GetByIdAsync(id);
            if (table == null) return NotFound();
            return Ok(table);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTableDto dto)
        {
            var table = new Table 
            { 
                RestaurantId = dto.RestaurantId,
                TableNumber = dto.TableNumber,
                Capacity = dto.Capacity,
                Status = TableStatus.Available
            };

            await _repository.AddAsync(table);
            return CreatedAtAction(nameof(GetById), new { id = table.Id }, table);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Replace(Guid id, [FromBody] PutTableDto dto)
        {
            var table = await _repository.GetByIdAsync(id);
            if (table == null) return NotFound();

            table.TableNumber = dto.TableNumber;
            table.Capacity = dto.Capacity;
            table.Status = dto.Status;

            await _repository.UpdateAsync(table);
            return Ok(table);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTableStatusDto dto)
        {
            var table = await _repository.GetByIdAsync(id);
            if (table == null) return NotFound();

            table.Status = dto.Status;
            await _repository.UpdateAsync(table);
            return Ok(table);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}