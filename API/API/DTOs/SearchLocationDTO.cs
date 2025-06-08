using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SearchLocationDTO
    {
        [Required] 
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }

        [Required]
        [Range(0.1, 100)] 
        public double KmRange { get; set; } // Search radius in km
        
        
    }
}
