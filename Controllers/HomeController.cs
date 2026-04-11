using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var role   = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (string.IsNullOrEmpty(role))
            return View(); // not logged in — show welcome

        if (role == "Manager")
        {
            ViewBag.Rooms = await _context.Rooms.ToListAsync();
            ViewBag.Tasks = await _context.CleaningTasks
                .Include(t => t.Room)
                .Include(t => t.AssignedUser)
                .ToListAsync();
        }
        else
        {
            ViewBag.MyTasks = await _context.CleaningTasks
                .Include(t => t.Room)
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }

        return View();
    }

    // Daily report — managers only
    [RequireRole("Manager")]
    public async Task<IActionResult> DailyReport()
    {
        var today = DateTime.UtcNow.Date;

        ViewBag.Rooms = await _context.Rooms.ToListAsync();
        ViewBag.Tasks = await _context.CleaningTasks
            .Include(t => t.Room)
            .Include(t => t.AssignedUser)
            .Where(t => t.CreatedAt.Date == today)
            .ToListAsync();

        return View();
    }

    // Cleaning time analytics — managers only
    [RequireRole("Manager")]
    public async Task<IActionResult> CleaningAnalytics()
    {
        var completedTasks = await _context.CleaningTasks
            .Include(t => t.Room)
            .Include(t => t.AssignedUser)
            .Where(t => t.Status == "Completed"
                     && t.AssignedAt.HasValue
                     && t.CompletedAt.HasValue)
            .OrderByDescending(t => t.CompletedAt)
            .Take(50)
            .ToListAsync();

        ViewBag.Tasks = completedTasks;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
