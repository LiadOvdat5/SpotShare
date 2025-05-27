using API.Data;
using API.DTOs;
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
        public async Task<Garage> CreateGarageAsync(CreateGarageDTO garageDTO, Guid userId)
        {
            var garage = new Garage
            {
                Id = Guid.NewGuid(),
                OwnerId = userId,
                DateCreated = DateTime.UtcNow,
                IsActive = true, // Assuming new garages are active by default

                Title = garageDTO.Title,
                Description = garageDTO.Description,
                Address = garageDTO.Address,
                Latitude = garageDTO.Latitude,
                Longitude = garageDTO.Longitude,
                ImageUrl = garageDTO.ImageUrl,
                PricePerHour = garageDTO.PricePerHour,
                
            };


            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            return garage;
        }

        /// <summary>
        /// Update an existing garage.
        /// </summary>
        public async Task<Garage> UpdateGarageAsync(Guid garageId, GarageUpdateDTO garageDTO, Guid userId)
        {
            // Check if garage owner is the user
            var garage = await _context.Garages.FindAsync(garageId);
            if (garage == null || garage.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this garage.");
            }

            // Update properties
            garage.Title = garageDTO.Title ?? garage.Title;
            garage.Description = garageDTO.Description ?? garage.Description;
            garage.Address = garageDTO.Address ?? garage.Address;
            garage.Latitude = garageDTO.Latitude ?? garage.Latitude;
            garage.Longitude = garageDTO.Longitude ?? garage.Longitude;
            garage.ImageUrl = garageDTO.ImageUrl ?? garage.ImageUrl;
            garage.PricePerHour = garageDTO.PricePerHour ?? garage.PricePerHour;
            garage.IsActive = garageDTO.IsActive ?? garage.IsActive;

            _context.Garages.Update(garage);
            await _context.SaveChangesAsync();

            return garage;

        }

        public async Task<bool> DeleteGarageAsync(Guid garageId, Guid userId)
        {
            var garage = await _context.Garages.FindAsync(garageId);
            if (garage == null || garage.OwnerId != userId)
            {
                return false; // Garage not found or user does not own it
            }

            _context.Garages.Remove(garage);
            await _context.SaveChangesAsync();
            return true; // Garage deleted successfully
        }
    }
    
}
