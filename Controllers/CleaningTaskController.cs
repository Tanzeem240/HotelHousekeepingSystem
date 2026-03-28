using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Controllers
{
    public class CleaningTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CleaningTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _context.CleaningTasks
                .Include(t => t.Room)
                .Include(t => t.AssignedUser) // 🔥 IMPORTANT
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            ViewBag.Users = users;

            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var task = await _context.CleaningTasks.Include(t => t.Room).FirstOrDefaultAsync(t => t.Id == id);
            if (task != null && task.Room != null)
            {
                task.IsInspected = true;
                task.Status = "Completed"; 
                task.Room.Status = "Available"; 
                await _context.SaveChangesAsync();
            }
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
        
        [HttpPost]
        public async Task<IActionResult> Assign(int taskId, int userId)
        {
            var task = await _context.CleaningTasks.FindAsync(taskId);

            if (task != null)
            {
                task.AssignedUserId = userId;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}