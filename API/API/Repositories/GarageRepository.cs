using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{

    public class GarageRepository : IGarageRepository
    {
        private readonly SpotShareDBContext _context;
        public GarageRepository(SpotShareDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all garages.
        /// </summary>
        public async Task<IEnumerable<Garage>> GetGaragesByUserIdAsync(Guid userId)
        {
            return await _context.Garages
                .Where(g => g.OwnerId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Create a new garage.
        /// </summary>
        // TODO: fIX after setting authentication.
        public async Task<Garage> CreateGarageAsync(Garage garage)
        {
            garage.Id = Guid.NewGuid();
            garage.DateCreated = DateTime.UtcNow;

            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            return garage;
        }



    }
    
}
