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

        // Assignment feature
        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        // NEW: track when task was assigned and when it was completed
        public DateTime? AssignedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Computed: how long cleaning took (in minutes)
        public double? CleaningDurationMinutes =>
            (AssignedAt.HasValue && CompletedAt.HasValue)
                ? (CompletedAt.Value - AssignedAt.Value).TotalMinutes
                : null;

        // Relationship
        public virtual Room? Room { get; set; }
    }
}