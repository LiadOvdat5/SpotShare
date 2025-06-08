using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SearchTimeDTO
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } // e.g. 08:00  

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; } // e.g. 18:00  
    }
}
