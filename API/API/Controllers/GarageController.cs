using API.Data;
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

        public GarageController(SpotShareDBContext context)
        {
            _context = context;
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

    }
}
