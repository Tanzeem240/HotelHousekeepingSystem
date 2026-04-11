using Microsoft.AspNetCore.Mvc;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelHousekeepingSystem.Controllers;

[RequireRole]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Workers page — manager only, shows ALL users with profile details
    [RequireRole("Manager")]
    public IActionResult Index()
    {
        var users = _context.Users
            .Include(u => u.WorkerProfile)
            .OrderBy(u => u.Role)
            .ThenBy(u => u.Name)
            .ToList();
        return View(users);
    }

    [RequireRole("Manager")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [RequireRole("Manager")]
    public IActionResult Create(User user)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(user);
    }

    public IActionResult ClockIn()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Auth");

        // Check if already clocked in (open session exists)
        bool alreadyClockedIn = _context.WorkSessions
            .Any(s => s.UserId == userId && s.ClockOutTime == null);

        if (!alreadyClockedIn)
        {
            _context.WorkSessions.Add(new WorkSession
            {
                UserId      = userId.Value,
                ClockInTime = DateTime.UtcNow
            });
            _context.SaveChanges();
        }

        return RedirectToAction("WorkSummary", "User");
    }

    public IActionResult ClockOut()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Auth");

        // Find the open session
        var session = _context.WorkSessions
            .FirstOrDefault(s => s.UserId == userId && s.ClockOutTime == null);

        if (session != null)
        {
            session.ClockOutTime = DateTime.UtcNow;
            _context.SaveChanges();
        }

        return RedirectToAction("WorkSummary", "User");
    }

    public IActionResult WorkSummary()
    {
        var userId   = HttpContext.Session.GetInt32("UserId");
        var role     = HttpContext.Session.GetString("UserRole");
        var userName = HttpContext.Session.GetString("UserName");

        if (userId == null) return RedirectToAction("Login", "Auth");

        if (role == "Manager")
        {
            // Manager's own open session (for clock in/out card)
            var managerOpenSession = _context.WorkSessions
                .FirstOrDefault(s => s.UserId == userId && s.ClockOutTime == null);

            // All sessions for all staff
            var allSessions = _context.WorkSessions
                .Include(s => s.User)
                .OrderByDescending(s => s.ClockInTime)
                .ToList();

            ViewBag.AllSessions = allSessions;
            ViewBag.OpenSession = managerOpenSession;
            ViewBag.IsManager   = true;
        }
        else
        {
            // Workers see only their own sessions
            var mySessions = _context.WorkSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.ClockInTime)
                .ToList();

            var openSession = mySessions.FirstOrDefault(s => s.ClockOutTime == null);
            var today       = DateTime.UtcNow.Date;
            var thisWeek    = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);

            double todayHours = mySessions
                .Where(s => s.ClockInTime.Date == today && s.ClockOutTime.HasValue)
                .Sum(s => (s.ClockOutTime!.Value - s.ClockInTime).TotalHours);

            double weekHours = mySessions
                .Where(s => s.ClockInTime.Date >= thisWeek && s.ClockOutTime.HasValue)
                .Sum(s => (s.ClockOutTime!.Value - s.ClockInTime).TotalHours);

            double totalHours = mySessions
                .Where(s => s.ClockOutTime.HasValue)
                .Sum(s => (s.ClockOutTime!.Value - s.ClockInTime).TotalHours);

            ViewBag.MySessions   = mySessions;
            ViewBag.OpenSession  = openSession;
            ViewBag.TodayHours   = Math.Round(todayHours, 2);
            ViewBag.WeekHours    = Math.Round(weekHours, 2);
            ViewBag.TotalHours   = Math.Round(totalHours, 2);
            ViewBag.IsManager    = false;
            ViewBag.UserName     = userName;
        }

        return View();
    }
}
