using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class BookingCreateDTO
    {
        [Required]
        public Guid GarageId { get; set; } // FK → Garages.Id

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } // Rental start date

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } // Rental end date

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } // Rental start time

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; } // Rental end time
    }
}
