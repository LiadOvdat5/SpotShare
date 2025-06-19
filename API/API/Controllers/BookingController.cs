using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;
        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepo = bookingRepository;
        }


        /// <summary>
        /// Method: Get
        /// Endpoint: `/api/bookings`
        /// Description: Get bookings details related to user.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Booking>>> GetBookings()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdString);
            
            
            var bookings = await _bookingRepo.GetBookingsByUserIdAsync(userId);
            if (bookings == null)
                return NotFound("No bookings found for this user.");
            return Ok(bookings);
        }

        /// <summary>
        /// Method: Get
        /// Endpoint: `/api/bookings/{bookingId}`
        /// Description: Get booking details by ID.
        /// </summary>
        [HttpGet("{bookingId}")]
        [Authorize]
        public async Task<ActionResult<Booking>> GetBooking(Guid bookingId)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null)
                return NotFound("Booking not found.");
            return Ok(booking);
        }


        /// <summary>
        /// Method: Post
        /// Endpoint: `/api/bookings`
        /// Description: Create a new booking.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateBooking([FromBody] BookingCreateDTO bookingDTO)
        {
            if (bookingDTO == null)
            {
                return BadRequest("Booking data is required.");
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();

            Guid userId = Guid.Parse(userIdString);

            // Call repository to create booking
            var resultBooking = await _bookingRepo.CreateBookingAsync(bookingDTO, userId);
            if (resultBooking == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating booking.");
            
            return CreatedAtAction(nameof(GetBooking), new { id = resultBooking.Id }, resultBooking);
            
        }

        /// <summary>
        /// Method: Delete
        /// Endpoint: `/api/bookings/{bookingId}/cancel`
        /// Description: Cancel a booking by ID.
        /// </summary>
        [HttpDelete("{bookingId}/cancel")]
        [Authorize]
        public async Task<ActionResult> DeleteBooking(Guid bookingId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdString);

            var result = await _bookingRepo.DeleteBookingAsync(bookingId, userId);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting booking.");

            return NoContent();
        }


        // Next requests are for Garage Owners (users with garages)

        /// <summary>
        /// Method: Patch
        /// Endpoint: `/api/bookings/{bookingId}/approve`
        /// Description: Approve a booking by ID.
        /// </summary>
        [HttpPatch("{bookingId}/approve")]
        [Authorize]
        public async Task<ActionResult> ApproveBooking(Guid bookingId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid ownerId = Guid.Parse(userIdString);

            var result = await _bookingRepo.ApproveBookingAsync(bookingId, ownerId);
            if (!result)
                return NotFound("Booking not found or not assigned to user.");
            
            return Ok("Booking Approved");
        }

        /// <summary>
        /// Method: Patch
        /// Endpoint: `/api/bookings/{bookingId}/reject`
        /// Description: Reject a booking by ID.
        /// </summary>
        [HttpPatch("{bookingId}/reject")]
        [Authorize]
        public async Task<ActionResult> RejectBooking(Guid bookingId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid ownerId = Guid.Parse(userIdString);

            var result = await _bookingRepo.RejectBookingAsync(bookingId, ownerId);
            if (!result)
                return NotFound("Booking not found or not assigned to user.");

            return Ok("Booking Rejected");
        }

        /// <summary>
        /// Method: Get
        /// Endpoint: `/api/myPendingRequests`
        /// Description: Get all pending requests for the user.
        /// </summary>
        [HttpGet("/myPendingRequets")]
        [Authorize]
        public async Task<ActionResult<List<Booking>>> GetPendingRequests()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdString);

            var bookings = await _bookingRepo.GetPendingBookings(userId);
            if (bookings == null || !bookings.Any(b => b.Status == BookingStatus.Pending))
                return NotFound("No pending requests found for this user.");

            return Ok(bookings.Where(b => b.Status == BookingStatus.Pending).ToList());
        }

    }
}
