using Microsoft.AspNetCore.Mvc;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelHousekeepingSystem.Controllers;

[RequireRole]
public class WorkerProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public WorkerProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    // My Profile — always shows the logged-in user's OWN profile
    public IActionResult Index()
    {
        var name   = HttpContext.Session.GetString("UserName");
        var userId = HttpContext.Session.GetInt32("UserId");

        var profile = _context.WorkerProfiles
            .Include(p => p.User)
            .FirstOrDefault(p => p.UserId == userId);

        if (profile == null)
            return RedirectToAction("Create");

        return View("Details", profile);
    }

    // GET: WorkerProfile/Details/5  (Manager views a specific worker)
    [RequireRole("Manager")]
    public IActionResult Details(int id)
    {
        var profile = _context.WorkerProfiles
            .Include(p => p.User)
            .FirstOrDefault(p => p.Id == id);

        if (profile == null) return NotFound();
        return View(profile);
    }

    // GET: WorkerProfile/Create
    public IActionResult Create()
    {
        var name   = HttpContext.Session.GetString("UserName");
        var role   = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        // Workers can only create their own profile
        if (role != "Manager")
        {
            bool exists = _context.WorkerProfiles.Any(p => p.UserId == userId);
            if (exists) return RedirectToAction("Index");

            ViewBag.UserId   = userId;
            ViewBag.UserName = name;
            return View();
        }

        // Manager: show ALL users who don't have a profile yet
        var usersWithoutProfile = _context.Users
            .Where(u => !_context.WorkerProfiles.Any(p => p.UserId == u.Id))
            .ToList();

        ViewBag.Workers = usersWithoutProfile;
        return View();
    }

    // POST: WorkerProfile/Create
    [HttpPost]
    public IActionResult Create(int UserId, string Nationality, string PhoneNumber, string Email)
    {
        var role          = HttpContext.Session.GetString("UserRole");
        var sessionUserId = HttpContext.Session.GetInt32("UserId");

        // Workers always use their own UserId from session
        int resolvedUserId = (role != "Manager") ? (sessionUserId ?? 0) : UserId;

        // Prevent duplicates
        bool exists = _context.WorkerProfiles.Any(p => p.UserId == resolvedUserId);
        if (exists)
        {
            TempData["Error"] = "A profile already exists for this worker.";
            return RedirectToAction("Index");
        }

        var profile = new WorkerProfile
        {
            UserId      = resolvedUserId,
            Nationality = Nationality,
            PhoneNumber = PhoneNumber,
            Email       = Email
        };

        _context.WorkerProfiles.Add(profile);
        _context.SaveChanges();
        TempData["Success"] = "Profile saved successfully.";
        return RedirectToAction("Index");
    }

    // GET: WorkerProfile/Edit/5
    public IActionResult Edit(int id)
    {
        var role   = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        var profile = _context.WorkerProfiles
            .Include(p => p.User)
            .FirstOrDefault(p => p.Id == id);

        if (profile == null) return NotFound();

        // Workers can only edit their own profile
        if (role != "Manager" && profile.UserId != userId)
            return RedirectToAction("AccessDenied", "Auth");

        return View(profile);
    }

    // POST: WorkerProfile/Edit/5
    [HttpPost]
    public IActionResult Edit(int id, string Nationality, string PhoneNumber, string Email)
    {
        var role   = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        var profile = _context.WorkerProfiles.Find(id);
        if (profile == null) return NotFound();

        if (role != "Manager" && profile.UserId != userId)
            return RedirectToAction("AccessDenied", "Auth");

        profile.Nationality = Nationality;
        profile.PhoneNumber = PhoneNumber;
        profile.Email       = Email;

        _context.SaveChanges();
        TempData["Success"] = "Profile updated successfully.";
        return RedirectToAction("Index");
    }
}
