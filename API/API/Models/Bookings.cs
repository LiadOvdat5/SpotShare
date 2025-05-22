namespace API.Models
{
    public class Bookings
    {
        public int Id { get; set; }
        public Guid GarageId { get; set; } // FK → Garages.Id
        public Guid RenterId { get; set; } // FK → Users.Id (Parking Searcher)
        public DateTime StartDateTime { get; set; } // Rental start
        public DateTime EndDateTime { get; set; } // Rental end
        public decimal TotalPrice { get; set; } // Calculated
        public BookingStatus Status { get; set; } // Pending, Approved, Completed, etc.
        public DateTime DateRequested { get; set; } // When booked
        public DateTime DateUpdated { get; set; } // Last status update
    }

    public enum BookingStatus
    {
        Pending,
        Approved,
        Completed,
        Canceled,
        Rejected
    }
}
