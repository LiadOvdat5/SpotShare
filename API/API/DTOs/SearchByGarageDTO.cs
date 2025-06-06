using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SearchByGarageDTO
    {
        [Required]
        public Guid GarageId { get; set; } // Unique identifier for the garage

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDateTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } // e.g. 08:00  

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; } // e.g. 18:00 
    }
}
