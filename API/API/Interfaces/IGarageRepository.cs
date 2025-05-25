using API.Models;

namespace API.Interfaces
{
    public interface IGarageRepository
    {

        Task<IEnumerable<Garage>> GetGaragesByUserIdAsync(Guid userId);

        Task<Garage> CreateGarageAsync(Garage garage);

    }
}
