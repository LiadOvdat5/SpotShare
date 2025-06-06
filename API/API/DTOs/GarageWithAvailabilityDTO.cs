using API.Models;

namespace API.DTOs
{
    public class GarageWithAvailabilityDTO
    {
        public GarageDTO Garage { get; set; } = default!;
        public List<AvailabilitySlot> MatchingSlots { get; set; } = new();
    }
}
