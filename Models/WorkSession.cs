namespace HotelHousekeepingSystem.Models;

public class WorkSession
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime ClockInTime { get; set; }
    public DateTime? ClockOutTime { get; set; }
}