using HotelHousekeepingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class WorkerProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public WorkerProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var role = HttpContext.Session.GetString("UserRole");
        var name = HttpContext.Session.GetString("UserName");

        var user = _context.Users.FirstOrDefault(u => u.Name == name);

        if (user == null) return NotFound();

        if (role == "Manager")
        {
            var profiles = _context.WorkerProfiles
                .Include(p => p.User)
                .ToList();

            return View(profiles);
        }
        else
        {
            var profile = _context.WorkerProfiles
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == user.Id);

            return View("Details", profile);
        }
    }
}