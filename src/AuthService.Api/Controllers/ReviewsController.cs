using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _repository;

        public ReviewsController(IReviewRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("restaurant/{restaurantId}")]
        [AllowAnonymous] // Cualquiera puede ver las reseñas
        public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
        {
            var reviews = await _repository.GetByRestaurantIdAsync(restaurantId);
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (dto.Rating < 1 || dto.Rating > 5) 
                return BadRequest("La calificación debe estar entre 1 y 5.");

            var review = new Review 
            { 
                CustomerId = Guid.Parse(userIdClaim),
                RestaurantId = dto.RestaurantId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await _repository.AddAsync(review);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var review = await _repository.GetByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}