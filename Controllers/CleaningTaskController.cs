using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Controllers;

[RequireRole]
public class CleaningTaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public CleaningTaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var role   = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");
        bool isManager = role == "Manager";

        IQueryable<CleaningTask> query = _context.CleaningTasks
            .Include(t => t.Room)
            .Include(t => t.AssignedUser);

        if (!isManager)
            query = query.Where(t => t.AssignedUserId == userId);

        var tasks = await query.ToListAsync();

        if (isManager)
            ViewBag.Users = await _context.Users.ToListAsync();

        ViewBag.IsManager     = isManager;
        ViewBag.CurrentUserId = userId;

        return View(tasks);
    }

    [HttpPost]
    [RequireRole("Manager")]
    public async Task<IActionResult> Assign(int taskId, int userId)
    {
        var task = await _context.CleaningTasks.FindAsync(taskId);
        if (task == null) return NotFound();
        if (task.AssignedUserId != null) return BadRequest("Already assigned");

        task.AssignedUserId = userId;
        task.Status     = "Assigned";
        task.AssignedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [RequireRole("Worker")]
    public async Task<IActionResult> MarkCleaned(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var task = await _context.CleaningTasks.FindAsync(id);
        if (task == null) return NotFound();
        if (task.AssignedUserId != userId) return Unauthorized();
        if (task.Status == "Ready" || task.Status == "Completed") return BadRequest("Already cleaned");

        task.Status = "Ready";
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [RequireRole("Manager")]
    public async Task<IActionResult> Approve(int id)
    {
        var task = await _context.CleaningTasks
            .Include(t => t.Room)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null || task.Room == null) return NotFound();
        if (task.AssignedUserId == null) return BadRequest("Task must be assigned first");
        if (task.Status == "Completed") return BadRequest("Already completed");

        task.IsInspected = true;
        task.Status      = "Completed";
        task.CompletedAt = DateTime.UtcNow;
        task.Room.Status = "Available";
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ReportIssue(int id, string notes)
    {
        var task = await _context.CleaningTasks.FindAsync(id);
        if (task != null)
        {
            task.MaintenanceNotes = notes;
            task.Status = "Maintenance Required";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
