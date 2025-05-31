using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class GarageUpdateDTO
    {
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string? Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string? ImageUrl { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price per hour must be greater than zero")]
        public decimal? PricePerHour { get; set; }
        public bool? IsActive { get; set; }
    }
}
