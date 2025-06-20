using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly SpotShareDBContext _context;
        public BookingRepository(SpotShareDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method: GetBookingsByUserIdAsync
        /// Description: Retrieves all bookings made by a specific user.
        /// </summary>
        public async Task<List<Booking>> GetBookingsByUserIdAsync(Guid userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.RenterId == userId)
                .ToListAsync();

            if (bookings == null || !bookings.Any())
                return null;

            return bookings;
        }


        public Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return null;

            return booking;
        }
        public Task<Booking> CreateBookingAsync(BookingCreateDTO bookingDTO, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBookingAsync(Guid bookingId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApproveBookingAsync(Guid bookingId, Guid ownerId)
        {
            throw new NotImplementedException();
        }



        public Task<List<Booking>> GetPendingBookings(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RejectBookingAsync(Guid bookingId, Guid ownerId)
        {
            throw new NotImplementedException();
        }
    }
}
