namespace API.DTOs
{
    public class GarageDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string? ImageUrl { get; set; }
        public decimal PricePerHour { get; set; }

    }
}
