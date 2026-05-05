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
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _repository;

        public ReservationsController(IReservationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetMyReservations()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var reservations = await _repository.GetByCustomerIdAsync(Guid.Parse(userIdClaim));
            return Ok(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var reservation = new Reservation 
            { 
                CustomerId = Guid.Parse(userIdClaim),
                RestaurantId = dto.RestaurantId,
                ReservationDate = dto.ReservationDate,
                PeopleCount = dto.PeopleCount,
                SpecialRequests = dto.SpecialRequests,
                Status = ReservationStatus.Pending
            };

            await _repository.AddAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Replace(Guid id, [FromBody] PutReservationDto dto)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null) return NotFound();

            reservation.ReservationDate = dto.ReservationDate;
            reservation.PeopleCount = dto.PeopleCount;
            reservation.Status = dto.Status;
            reservation.SpecialRequests = dto.SpecialRequests;

            await _repository.UpdateAsync(reservation);
            return Ok(reservation);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateReservationStatusDto dto)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null) return NotFound();

            reservation.Status = dto.Status;
            await _repository.UpdateAsync(reservation);
            return Ok(reservation);
        }
    }
}