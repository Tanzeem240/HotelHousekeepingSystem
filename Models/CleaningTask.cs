using System.ComponentModel.DataAnnotations;

namespace HotelHousekeepingSystem.Models;

public class CleaningTask
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public Room? Room { get; set; }

    public int AssignedToUserId { get; set; }

    public User? AssignedToUser { get; set; }

    public string Status { get; set; } = "Pending";
}