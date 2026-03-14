using HotelHousekeepingSystem.Data;
using Microsoft.AspNetCore.Mvc;

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
        var tasks = _context.CleaningTasks.ToList();
        return View(tasks);
    }
}