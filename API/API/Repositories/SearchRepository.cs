using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        public SpotShareDBContext _context;
        public SearchRepository(SpotShareDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Calculates the distance between two geographical points using the Haversine formula.
        /// </summary>
        public double GetDistanceInKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the earth in km
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }


        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        private double DegreesToRadians(double deg) => deg * (Math.PI / 180);

        /// <summary>
        /// Searches for garages within a specified range and availability based on the provided date & time.
        /// </summary>
        /*
        public async Task<List<GarageWithAvailabilityDTO>> SearchGarages(SearchLocationDTO searchLocationDto, SearchTimeDTO searchTimeDto)
        {
            
            List<GarageDTO> garagesInRangeDTO = await GetGaragesWithinRange(searchLocationDto);


            // Extract just the garage IDs
            var garageIds = garagesInRangeDTO.Select(g => g.Id).ToList();

            // Use the list of IDs in the LINQ query (this can be translated to SQL)
            var availabilitySlots = await _context.AvailabilitySlots
                .Where(slot => garageIds.Contains(slot.GarageId))
                .ToListAsync();

            if (availabilitySlots == null || availabilitySlots.Count == 0)
                return null;


            // Filter Availability Slots based on the provided date and time range
            var filteredSlots = availabilitySlots
                .Where(slot => slot.StartDate <= searchTimeDto.StartDate &&
                               slot.EndDate >= searchTimeDto.EndDate &&
                               slot.StartTime <= searchTimeDto.StartTime &&
                               slot.EndTime >= searchTimeDto.EndTime)
                .ToList();

            if (filteredSlots == null || filteredSlots.Count == 0)
                return null;


            // Remove garages that have no availability slots in the filtered list
            garagesInRangeDTO = garagesInRangeDTO
                .Where(garage => filteredSlots.Any(slot => slot.GarageId == garage.Id))
                .ToList();

            // TODO: Filter Availability Slots with booked status (After Booking Implementation) 



            // Create list of GarageWithAvailabilityDTO to return
            var garageWithAvailabilityList = garagesInRangeDTO.Select(garageDTO => new GarageWithAvailabilityDTO
            {
                Garage = garageDTO,
                MatchingSlots = filteredSlots
       .Where(slot => slot.GarageId == garageDTO.Id).ToList()
            }).ToList();

            return garageWithAvailabilityList;

        }
        */

        public async Task<List<GarageWithAvailabilityDTO>> SearchGarages(SearchLocationDTO searchLocationDto, SearchTimeDTO searchTimeDto)
        {
            // Get garages in location range
            List<GarageDTO> garagesInRangeDTO = await GetGaragesWithinRange(searchLocationDto);
            var garageIds = garagesInRangeDTO.Select(g => g.Id).ToList();

            // Prepare list of search dates
            var searchDates = Enumerable.Range(0, (searchTimeDto.EndDate - searchTimeDto.StartDate).Days + 1)
                .Select(offset => searchTimeDto.StartDate.Date.AddDays(offset))
                .ToList();

            // Fetch availability slots within those dates and time range
            var availabilitySlots = await _context.AvailabilitySlots
                .Where(slot =>
                    garageIds.Contains(slot.GarageId) &&
                    searchDates.Contains(slot.Date.Date) &&
                    slot.StartTime <= searchTimeDto.StartTime &&
                    slot.EndTime >= searchTimeDto.EndTime
                ).ToListAsync();

            // Group slots by garage
            var result = garagesInRangeDTO
                .Select(garage => new GarageWithAvailabilityDTO
                {
                    Garage = garage,
                    MatchingSlots = availabilitySlots
                        .Where(slot => slot.GarageId == garage.Id)
                        .OrderBy(slot => slot.Date)
                        .ToList()
                })
                .Where(g => g.MatchingSlots.Any())
                .ToList();

            return result;
        }


        /// <summary>
        /// Retrieves all active garages within the specified range from the search DTO.
        /// </summary>
        public async Task<List<GarageDTO>> GetGaragesWithinRange(SearchLocationDTO searchLocationDto)
        {
            // Get all active garages from the database
            var allGarages = await _context.Garages
            .Where(g => g.IsActive)
            .ToListAsync();

            //Console.WriteLine($"Total Garages Found: {allGarages.Count}");

            if (allGarages == null || allGarages.Count == 0)
                return null;

            // Filter by the distance using the Haversine formula
            var garagesInRange = allGarages
            .Where(g => GetDistanceInKm(searchLocationDto.Latitude, searchLocationDto.Longitude, g.Latitude, g.Longitude) <= searchLocationDto.KmRange)
            .ToList();
            if (garagesInRange == null || garagesInRange.Count == 0)
                return null;

            var garagesInRangeDTO = garagesInRange.Select(g => new GarageDTO
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                Address = g.Address,
                Latitude = g.Latitude,
                Longitude = g.Longitude,
                ImageUrl = g.ImageUrl,
                PricePerHour = g.PricePerHour
            }).ToList();

            return garagesInRangeDTO;
        }

        /// <summary>
        /// Searches for available slots for a specific garage based on the provided date and time.
        /// </summary>
        public async Task<GarageWithAvailabilityDTO> SearchSlotsForGarageWithTime(Guid garageId, SearchTimeDTO searchTimeDto)
        {
            // Get the garage by ID
            var garage = await _context.Garages
                .Where(g => g.Id == garageId && g.IsActive)
                .FirstOrDefaultAsync();
            
            if (garage == null)
                return null;

            // Build list of dates in the range
            var searchDates = Enumerable.Range(0, (searchTimeDto.EndDate - searchTimeDto.StartDate).Days + 1)
                .Select(offset => searchTimeDto.StartDate.Date.AddDays(offset))
                .ToList();

            // Get all availability slots for those dates
            var availabilitySlots = await _context.AvailabilitySlots
                .Where(slot => slot.GarageId == garageId && searchDates.Contains(slot.Date))
                .ToListAsync();

            if (availabilitySlots == null || availabilitySlots.Count == 0)
                return null;

            // Filter availability slots by time range
            var filteredSlots = availabilitySlots
                .Where(slot =>
                    slot.StartTime <= searchTimeDto.StartTime &&
                    slot.EndTime >= searchTimeDto.EndTime)
                .ToList();

            if (filteredSlots == null || filteredSlots.Count == 0)
                return null;

            // create GarageWithAvailabilityDTO object to return
            var garageWithAvailability = new GarageWithAvailabilityDTO
            {
                Garage = new GarageDTO
                {
                    Id = garage.Id,
                    Title = garage.Title,
                    Description = garage.Description,
                    Address = garage.Address,
                    Latitude = garage.Latitude,
                    Longitude = garage.Longitude,
                    ImageUrl = garage.ImageUrl,
                    PricePerHour = garage.PricePerHour
                },
                MatchingSlots = filteredSlots
            };

            // return the GarageWithAvailabilityDTO object
            return garageWithAvailability;
        }
    }
}
