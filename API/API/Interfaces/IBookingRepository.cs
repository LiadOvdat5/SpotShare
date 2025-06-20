using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingAsync(BookingCreateDTO bookingDTO, Guid userId);
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
        Task<List<Booking>> GetBookingsByUserIdAsync(Guid userId);
        Task<bool> ApproveBookingAsync(Guid bookingId, Guid ownerId);
        Task<bool> RejectBookingAsync(Guid bookingId, Guid ownerId);
        Task<bool> DeleteBookingAsync(Guid bookingId, Guid userId);
        Task<List<Booking>> GetPendingBookings(Guid userId);
    }
}
