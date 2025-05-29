namespace API.DTOs
{
    public class CreateAvailabilitySlotDTO
    {
        public DateTime StartDate { get; set; } // Start date
        public DateTime EndDate { get; set; } // End date 
        public int DayOfWeek { get; set; } // 0 = Sunday, 6 = Saturday  
        public TimeSpan StartTime { get; set; } // e.g. 08:00  
        public TimeSpan EndTime { get; set; } // e.g. 18:00  
    }
}
