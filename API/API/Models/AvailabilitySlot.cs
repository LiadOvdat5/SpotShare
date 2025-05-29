namespace API.Models
{
    public class AvailabilitySlot
    {
        public Guid Id { get; set; } // PK  
        public Guid GarageId { get; set; } // FK → Garages.Id  
        public DateTime StartDate { get; set; } // Start date
        public DateTime EndDate { get; set; } // End date 
        public int DayOfWeek { get; set; } // 0 = Sunday, 6 = Saturday  
        public TimeSpan StartTime { get; set; } // e.g. 08:00  
        public TimeSpan EndTime { get; set; } // e.g. 18:00  

        // cance recurring, change of functionality (start&end date )
        //public bool IsRecurring { get; set; } // Repeats weekly?  
    }
}
