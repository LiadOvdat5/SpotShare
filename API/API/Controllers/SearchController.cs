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
        /// Endpoint: `/api/search`
        /// Description: Search for garages based on location, date and time. (SearchDTO)
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GarageWithAvailabilityDTO>>> SearchGarages([FromQuery] SearchDTO searchDto)
        {
            if (searchDto == null)
            {
                return BadRequest("Search parameters cannot be null.");
            }

            var result = await _searchRepo.SearchGarages(searchDto);

            if (result == null || result.Count == 0)
                return NotFound("No garages found matching the search criteria.");

            return Ok(result);

        }


    }
}
