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

        /// <summary>
        /// Method: GetBookingByIdAsync
        /// Description: Retrieves a booking by its unique identifier.
        /// </summary>
        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return null;

            return booking;
        }


        public async Task<Booking> CreateBookingAsync(BookingCreateDTO bookingDTO, Guid userId)
        {
            if(bookingDTO == null)
                return null;


            // Validate booking dates and times
            if (bookingDTO.StartDate < DateTime.Now || bookingDTO.EndDate < bookingDTO.StartDate)
                return null;

            if (bookingDTO.StartTime < TimeSpan.Zero || bookingDTO.EndTime < bookingDTO.StartTime)
                return null;


            var garage = await _context.Garages.FindAsync(bookingDTO.GarageId);
            if (garage == null)
                return null;

            // Check if the garage is available for the requested dates and times
            /*
            var existingSlot = await _context.AvailabilitySlots
                .Where(a => a.GarageId == bookingDTO.GarageId &&
                            (a.StartDate <= bookingDTO.EndDate && a.EndDate >= bookingDTO.StartDate) &&
                            (a.StartTime <= bookingDTO.EndTime && a.EndTime >= bookingDTO.StartTime))
                .FirstOrDefaultAsync();
            
            if(existingSlot == null)
                return null; // No available slot found
             */


            // Calculate total price based on hours and days
            decimal hoursPerDay = (decimal)(bookingDTO.EndTime - bookingDTO.StartTime).TotalHours;
            int numberOfDays = (bookingDTO.StartDate - bookingDTO.EndDate).Days + 1;
            decimal totalPrice = hoursPerDay * numberOfDays * garage.PricePerHour;

            var booking = new Booking {
                Id = new Guid(), 
                GarageId = bookingDTO.GarageId,
                RenterId = userId, 
                StartDate = bookingDTO.StartDate,
                EndDate = bookingDTO.EndDate,
                StartTime = bookingDTO.StartTime,
                EndTime = bookingDTO.EndTime,
                TotalPrice = totalPrice,
                Status = BookingStatus.Pending,
                DateRequested = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            // Split the availability slot 
            //TODO: create two function - split and add | for approve and cancel booking

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
