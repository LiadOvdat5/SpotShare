using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageController : ControllerBase
    {
        private readonly SpotShareDBContext _context;
        private readonly IGarageRepository _garageRepository;

        public GarageController(SpotShareDBContext context, IGarageRepository garageRepository)
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
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Garage>>> GetGaragesByUserId(Guid userId)
        {
            var garages = await _garageRepository.GetGaragesByUserIdAsync(userId);
            if (garages == null || !garages.Any())
            {
                return NotFound();
            }
            return Ok(garages);
        }

        

    }
}
