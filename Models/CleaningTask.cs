using System.ComponentModel.DataAnnotations;

namespace HotelHousekeepingSystem.Models
{
    public class CleaningTask
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        
        public string Status { get; set; } = "Pending"; 
        
        // Week 10 fields
        public string? MaintenanceNotes { get; set; }
        public bool IsInspected { get; set; } = false; 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //  (Assignment Feature)
        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        // Relationship
        public virtual Room? Room { get; set; }
    }
}