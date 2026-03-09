using System.ComponentModel.DataAnnotations;

namespace HotelHousekeepingSystem.Models;
    public class CleaningTask
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public string Status { get; set; } = "Pending";

        public int AssignedToUserId { get; set; }
    }
