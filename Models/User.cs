using System.ComponentModel.DataAnnotations;

namespace HotelHousekeepingSystem.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Role { get; set; }

    public DateTime? ClockInTime{ get; set; }
    
    public DateTime? ClockOutTime { get; set; }
    
    public WorkerProfile WorkerProfile { get; set; }
}