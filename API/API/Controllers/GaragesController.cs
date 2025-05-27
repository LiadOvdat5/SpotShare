using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GaragesController : ControllerBase
    {
        private readonly SpotShareDBContext _context;
        private readonly IGarageRepository _garageRepository;

        public GaragesController(SpotShareDBContext context, IGarageRepository garageRepository)
        {
            _context = context;
            _garageRepository = garageRepository;
        }

        /// <summary>
        /// Method: Get 
        /// Endpoint: `/api/garages`
        /// Description: Get all active garages (filtered list).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Garage>>> GetGarages()
        {
            return Ok(await _context.Garages.ToListAsync());
        }


        /// <summary>
        /// Method: Get 
        /// Endpoint: `/api/garages/{id}`
        /// Description: Get garage details by ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Garage>> GetGarage(Guid id)
        {
            var garage = await _context.Garages.FindAsync(id);
            if (garage == null)
            {
                return NotFound();
            }
            return Ok(garage);
        }

        /// <summary>
        /// Method: Get
        /// Endpoint: `/api/garages/user/{userId}`
        /// Description: Get all garages by user ID.
        /// </summary>
        [HttpGet("user")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<List<Garage>>> GetGaragesByUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();

            Guid userId = Guid.Parse(userIdString);

            var garages = await _garageRepository.GetGaragesByUserIdAsync(userId);
            if (garages == null || !garages.Any())
            {
                return NotFound();
            }
            return Ok(garages);
        }

        /// <summary>
        /// Method: Post
        /// Endpoint: `/api/garages`
        /// Description: Create a new garage.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Garage>> CreateGarage([FromBody] CreateGarageDTO garageDTO)
        {
            if (garageDTO == null)
            {
                return BadRequest("Garage data is null");
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();

            Guid userId = Guid.Parse(userIdString);

            var createdGarage = await _garageRepository.CreateGarageAsync(garageDTO, userId);
            return CreatedAtAction(nameof(GetGarage), new { id = createdGarage.Id }, createdGarage);
        }

        /// <summary>
        /// Method: Patch
        /// Endpoint: `/api/garages/{garageId}`
        /// Description: Update an existing garage.
        /// </summary>
        [HttpPatch("{garageId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateGarage(Guid garageId, [FromBody] GarageUpdateDTO updateGarageDTO)
        {
            // Check if the garage exists 
            if (garageId == Guid.Empty)
            {
                return BadRequest("Invalid garage ID");
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdString);

            var garage = await _garageRepository.UpdateGarageAsync(garageId, updateGarageDTO, userId);

            if (garage == null)
            {
                return NotFound("Garage not found or you do not have permission to update it.");
            }

            return Ok(garage);

        }

        /// <summary>
        /// Method: Delete
        /// Endpoint: `/api/garages/{garageId}`
        /// Description: Delete a garage by ID.
        /// </summary>
        [HttpDelete("{garageId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteGarage(Guid garageId)
        {
            if (garageId == Guid.Empty)
            {
                return BadRequest("Invalid garage ID");
            }
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdString);
            var result = await _garageRepository.DeleteGarageAsync(garageId, userId);
            if (!result)
            {
                return NotFound("Garage not found or you do not have permission to delete it.");
            }
            return NoContent();
        }

        /// <summary>
        /// Method: Post
        /// Endpoint: `/api/garages/{garageId}/availability`
        /// Description: Create availability slots for a garage.
        /// </summary>
        [HttpPost("{garageId}/availability")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<AvailabilitySlot>> CreateAvailabilitySlots(Guid garageId, [FromBody] CreateAvailabilitySlotDTO slotDTO)
        {
            if (slotDTO == null)
            {
                return BadRequest("Availability slot data is null");
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdString);

            var createdSlot = await _garageRepository.CreateAvailabilitySlotsAsync(garageId, slotDTO, userId);
            if (createdSlot == null)
            {
                return NotFound("Garage not found or you do not have permission to create availability slots.");
            }

            return CreatedAtAction(nameof(GetGarage), new { id = garageId }, createdSlot);
        }

        /// <summary>
        /// Method: Get
        /// Endpoint: `/api/garages/{garageId}/availability`
        /// Description: Get all availability slots for a garage.
        /// </summary>
        [HttpGet("{garageId}/availability")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<AvailabilitySlot>>> GetAvailabilitySlotsByGarageId(Guid garageId)
        {
            var slots = await _garageRepository.GetAvailabilitySlotsByGarageIdAsync(garageId);
            if (slots == null || !slots.Any())
            {
                return NotFound("No availability slots found for this garage.");
            }
            return Ok(slots);
        }

        /// <summary>
        /// Method: Delete
        /// Endpoint: `/api/garages/availability/{slotId}`
        /// Description: Delete an availability slot by ID.
        /// </summary>
        [HttpDelete("availability/{slotId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteAvailabilitySlot(Guid slotId)
        {
            if (slotId == Guid.Empty)
                return BadRequest("Invalid slot ID");

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdString);

            var result = await _garageRepository.DeleteAvailabilitySlot(slotId, userId);
            if (!result)
                return NotFound("Availability slot not found or you do not have permission to delete it.");
            
            return NoContent();
        }

    }
}
