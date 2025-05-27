namespace API.Models
{
    public class AvailabilitySlot
    {
        public Guid Id { get; set; } // PK  
        public Guid GarageId { get; set; } // FK → Garages.Id  
        public int DayOfWeek { get; set; } // 0 = Sunday, 6 = Saturday  
        public TimeSpan StartTime { get; set; } // e.g. 08:00  
        public TimeSpan EndTime { get; set; } // e.g. 18:00  
        public bool IsRecurring { get; set; } // Repeats weekly?  
    }
}
