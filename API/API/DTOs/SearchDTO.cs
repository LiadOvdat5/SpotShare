using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SearchDTO
    {
        [Required] 
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }

        [Required]
        [Range(0.1, 100)] 
        public double KmRange { get; set; } // Search radius in km
        
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
