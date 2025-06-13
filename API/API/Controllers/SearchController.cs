using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository _searchRepo;

        public SearchController(ISearchRepository searchRepo)
        {
            _searchRepo = searchRepo;
        }


        /// <summary>
        /// Method: GET
        /// Endpoint: `/api/search/withTime`
        /// Description: Search for garages based on location, date and time. (SearchLocationDTO, SearchTimeDTO)
        /// </summary>
        [HttpGet("withTime")]
        [Authorize]
        public async Task<ActionResult<List<GarageWithAvailabilityDTO>>> SearchGaragesByLocationAndTime([FromQuery] SearchLocationDTO searchLocationDto, [FromQuery] SearchTimeDTO searchTimeDto)
        {
            if (searchLocationDto == null)
            {
                return BadRequest("Search parameters cannot be null.");
            }

            var result = await _searchRepo.SearchGarages(searchLocationDto, searchTimeDto);

            if (result == null || result.Count == 0)
                return NotFound("No garages found matching the search criteria.");

            return Ok(result);

        }

        /// <summary>
        /// Method: GET
        /// Endpoint: `/api/search/range`
        /// Description: Get all garages within a specified range from a given location. (SearchLocationDTO)
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GarageDTO>>> GetGaragesWithinRange([FromQuery] SearchLocationDTO searchDto)
        {
            if (searchDto == null)
            {
                return BadRequest("Search parameters cannot be null.");
            }
            var garages = await _searchRepo.GetGaragesWithinRange(searchDto);
            if (garages == null || garages.Count == 0)
                return NotFound("No garages found within the specified range.");
            return Ok(garages);
        }

        /// <summary>
        /// Method: GET
        /// Endpoint: `/api/search/slotsForGarage`
        /// Description: Search for available slots for a specific garage based on the provided date and time. (GarageId, SearchTimeDTO)
        /// </summary>
        [HttpGet("slotsForGarage")]
        [Authorize]
        public async Task<ActionResult<GarageWithAvailabilityDTO>> SearchSlotsForGarage([FromQuery] Guid garageId, [FromQuery] SearchTimeDTO searchTimeDto)
        {
            if (garageId == Guid.Empty || searchTimeDto == null)
            {
                return BadRequest("Invalid garage ID or search time parameters.");
            }
            var result = await _searchRepo.SearchSlotsForGarageWithTime(garageId, searchTimeDto);

            if (result == null)
                return NotFound("No availability slots found for the specified garage.");

            return Ok(result);
        }

    }
}
