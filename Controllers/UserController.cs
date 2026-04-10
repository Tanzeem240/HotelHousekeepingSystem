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
        var name = HttpContext.Session.GetString("UserName");
        var user = _context.Users.FirstOrDefault(u => u.Name == name);
        if (user != null)
        {
            if (user.ClockInTime != null && user.ClockOutTime == null)
                return RedirectToAction("Index", "Room");
            user.ClockInTime  = DateTime.UtcNow;
            user.ClockOutTime = null;
            _context.SaveChanges();
        }
        return RedirectToAction("Index", "Room");
    }

    public IActionResult ClockOut()
    {
        var name = HttpContext.Session.GetString("UserName");
        var user = _context.Users.FirstOrDefault(u => u.Name == name);
        if (user != null)
        {
            if (user.ClockInTime == null || user.ClockOutTime != null)
                return RedirectToAction("Index", "Room");
            user.ClockOutTime = DateTime.UtcNow;
            _context.SaveChanges();
        }
        return RedirectToAction("Index", "Room");
    }

    public IActionResult WorkSummary()
    {
        var name = HttpContext.Session.GetString("UserName");
        var user = _context.Users.FirstOrDefault(u => u.Name == name);
        if (user != null && user.ClockInTime != null && user.ClockOutTime != null)
            ViewBag.WorkHours = Math.Round((user.ClockOutTime - user.ClockInTime)?.TotalHours ?? 0, 2);
        else
            ViewBag.WorkHours = 0;
        return View();
    }
}
