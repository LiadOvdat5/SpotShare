namespace API.DTOs
{
    public class CreateAvailabilitySlotDTO
    {
        public int DayOfWeek { get; set; } // 0 = Sunday, 6 = Saturday  
        public TimeSpan StartTime { get; set; } // e.g. 08:00  
        public TimeSpan EndTime { get; set; } // e.g. 18:00  
        public bool IsRecurring { get; set; } // Repeats weekly?  
    }
}
