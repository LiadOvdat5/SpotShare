namespace API.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid GarageId { get; set; } // FK → Garages.Id
        public Guid RenterId { get; set; } // FK → Users.Id (Parking Searcher)
        public DateTime StartDate { get; set; } // Rental start date
        public DateTime EndDate { get; set; } // Rental end date
        public TimeSpan StartTime { get; set; } // Rental start time
        public TimeSpan EndTime { get; set; } // Rental end time
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
