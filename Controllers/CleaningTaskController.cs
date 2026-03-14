using HotelHousekeepingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelHousekeepingSystem.Controllers;

public class CleaningTaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public CleaningTaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var tasks = _context.CleaningTasks
            .Include(t => t.Room)
            .ToList();

        return View(tasks);
    }
    
    public IActionResult Complete(int id)
    {
        var task = _context.CleaningTasks.Find(id);

        if (task != null)
        {
            task.Status = "Completed";

            var room = _context.Rooms.Find(task.RoomId);
            if (room != null)
            {
                room.Status = "Available";
            }

            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}