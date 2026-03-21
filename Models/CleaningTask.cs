using System.ComponentModel.DataAnnotations;

namespace HotelHousekeepingSystem.Models
{
    public class CleaningTask
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        
                public string Status { get; set; } = "Pending"; 
        
        // New fields for Week 10
        public string? MaintenanceNotes { get; set; } // Stores info on broken items 
        public bool IsInspected { get; set; } = false; // Supervisor approval flag 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //  property to link to the Room model
        public virtual Room? Room { get; set; }
    }
}