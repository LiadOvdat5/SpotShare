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

        /// <summary>
        /// Delete a garage.
        /// </summary>
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

        /// <summary>
        /// Create availability slots for a garage.
        /// </summary>
        public async Task<AvailabilitySlot> CreateAvailabilitySlotsAsync(Guid garageId, CreateAvailabilitySlotDTO slotDTO, Guid userId)
        {
            // Check if garage exists and belongs to the user
            var garage = await _context.Garages.FindAsync(garageId);
            if (garage == null || garage.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to create availability slots for this garage.");
            }

            // Validate the slotDTO properties
            if (slotDTO.StartDate >= slotDTO.EndDate)
            {
                throw new ArgumentException("Start date must be before end date.");
            }
            if (slotDTO.StartTime >= slotDTO.EndTime)
            {
                throw new ArgumentException("Start time must be before end time.");
            }
            if (slotDTO.DayOfWeek < 0 || slotDTO.DayOfWeek > 6)
            {
                throw new ArgumentException("Day of week must be between 0 (Sunday) and 6 (Saturday).");
            }
            if (slotDTO.StartDate.Date < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Start date cannot be in the past.");
            }


            // Check if another slot already exists on the same time and day  
            var existingSlot = await _context.AvailabilitySlots
               .Where(slot => slot.GarageId == garageId && slot.DayOfWeek == slotDTO.DayOfWeek) // same garage and day of week  
               .Where(slot => slot.StartDate <= slotDTO.EndDate && slot.EndDate >= slotDTO.StartDate) // between start and end date of the new slot
               .FirstOrDefaultAsync(slot =>
                   (slot.StartTime < slotDTO.EndTime && slot.EndTime > slotDTO.StartTime)); // overlapping time  

            if (existingSlot != null)
            {
                throw new InvalidOperationException($"An availability slot already exists for this garage on the same day and overlapping time. Slot ID: {existingSlot.Id}");
            }


            var availabilitySlot = new AvailabilitySlot
            {
                Id = Guid.NewGuid(),
                GarageId = garageId,
                StartDate = slotDTO.StartDate,
                EndDate = slotDTO.EndDate,
                DayOfWeek = slotDTO.DayOfWeek,
                StartTime = slotDTO.StartTime,
                EndTime = slotDTO.EndTime
            };
            
            _context.AvailabilitySlots.Add(availabilitySlot);
            await _context.SaveChangesAsync();

            return availabilitySlot;
        }

        /// <summary>
        /// Get availability slots by garage ID.
        /// </summary>
        public async Task<IEnumerable<AvailabilitySlot>> GetAvailabilitySlotsByGarageIdAsync(Guid garageId)
        {
            return await _context.AvailabilitySlots
                .Where(slot => slot.GarageId == garageId)
                .ToListAsync();
        }

        /// <summary>
        /// Delete an availability slot by ID.
        /// </summary>
        public async Task<bool> DeleteAvailabilitySlot(Guid slotId, Guid userId)
        {
            var slot = await _context.AvailabilitySlots.FindAsync(slotId);
            if (slot == null)
            {
                return false; // Slot not found 
            }

            var garage = await _context.Garages.FindAsync(slot?.GarageId);
            if (garage == null || garage.OwnerId != userId)
            {
                return false; // Garage not found or user does not own it
            }

            _context.AvailabilitySlots.Remove(slot);
            await _context.SaveChangesAsync();

            return true; // Slot deleted successfully
        }

    }
    
}
