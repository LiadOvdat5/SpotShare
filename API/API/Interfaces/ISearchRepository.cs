using API.DTOs;

namespace API.Interfaces
{
    public interface ISearchRepository
    {
        public double GetDistanceInKm(double lat1, double lon1, double lat2, double lon2);

        public Task<List<GarageWithAvailabilityDTO>> SearchGarages(SearchDTO searchDto);

    }
}
