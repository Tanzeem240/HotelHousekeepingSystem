using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelHousekeepingSystem.Controllers;
    public class CleaningTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }

            base.OnActionExecuting(context);
        }
        
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
            // ROLE CHECK
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Manager")
            {
                return Unauthorized(); // block workers
            }

            var task = await _context.CleaningTasks
                .Include(t => t.Room)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null || task.Room == null)
                return NotFound();

            //  VALIDATION
            if (task.AssignedUserId == null)
                return BadRequest("Task must be assigned first");

            if (task.Status == "Completed")
                return BadRequest("Already completed");

            task.IsInspected = true;
            task.Status = "Completed";
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
        
        [HttpPost]
        public async Task<IActionResult> Assign(int taskId, int userId)
        {
            // ROLE CHECK
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Manager")
            {
                return Unauthorized();
            }

            var task = await _context.CleaningTasks.FindAsync(taskId);

            if (task == null)
                return NotFound();

            // VALIDATION
            if (task.AssignedUserId != null)
                return BadRequest("Already assigned");

            task.AssignedUserId = userId;
            task.Status = "Assigned";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public async Task<IActionResult> MarkCleaned(int id)
        {
            var task = await _context.CleaningTasks.FindAsync(id);

            if (task == null)
                return NotFound();

            // Only allow if assigned
            if (task.AssignedUserId == null)
                return BadRequest("Task must be assigned first");

            // Prevent re-cleaning
            if (task.Status == "Ready" || task.Status == "Completed")
                return BadRequest("Already cleaned");

            task.Status = "Ready";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
