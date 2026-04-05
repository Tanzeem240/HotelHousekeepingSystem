namespace HotelHousekeepingSystem.Models;

public class WorkerProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Nationality { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}