using System.ComponentModel.DataAnnotations;

namespace HotelHousekeepingSystem.Models;

public class Room
{
    public int Id { get; set; }

    [Required]
    public int RoomNumber { get; set; }

    public string Status { get; set; } = "Available";

    // NEW: room category
    public string Category { get; set; } = "Regular";
}