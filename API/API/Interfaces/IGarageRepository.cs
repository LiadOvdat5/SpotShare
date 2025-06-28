using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    public interface IGarageRepository
    {

        Task<IEnumerable<Garage>> GetGaragesByUserIdAsync(Guid userId);
        Task<Garage> CreateGarageAsync(CreateGarageDTO garageDTO, Guid userId);
        Task<Garage> UpdateGarageAsync(Guid garageId, GarageUpdateDTO garageDTO, Guid userId);
        Task<bool> DeleteGarageAsync(Guid garageId, Guid userId);
        Task<List<AvailabilitySlot>> CreateAvailabilitySlotsAsync(Guid garageId, CreateAvailabilitySlotDTO slotDTO, Guid userId);
        Task<IEnumerable<AvailabilitySlot>> GetAvailabilitySlotsByGarageIdAsync(Guid garageId);
        Task<bool> DeleteAvailabilitySlot(Guid slotId, Guid userId);
    }
}
