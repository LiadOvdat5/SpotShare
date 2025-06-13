using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    public interface ISearchRepository
    {
        public double GetDistanceInKm(double lat1, double lon1, double lat2, double lon2);

        public Task<List<GarageWithAvailabilityDTO>> SearchGarages(SearchLocationDTO searchLocationDto, SearchTimeDTO searchTimeDto);

        public Task<List<GarageDTO>> GetGaragesWithinRange(SearchLocationDTO searchDto);

        public Task<GarageWithAvailabilityDTO> SearchSlotsForGarageWithTime(Guid garageId, SearchTimeDTO searchTimeDto);

    }
}
