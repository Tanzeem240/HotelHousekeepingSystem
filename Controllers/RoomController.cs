using Microsoft.AspNetCore.Mvc;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Controllers;

[RequireRole]
public class RoomController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var rooms = _context.Rooms.ToList();
        return View(rooms);
    }

    [RequireRole("Manager")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [RequireRole("Manager")]
    public IActionResult Create(Room room)
    {
        if (ModelState.IsValid)
        {
            room.Status = "Available";
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(room);
    }

    [RequireRole("Manager")]
    public IActionResult CheckIn(int id)
    {
        var room = _context.Rooms.Find(id);
        if (room != null)
        {
            room.Status = "Occupied";
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    [RequireRole("Manager")]
    public IActionResult CheckOut(int id)
    {
        var room = _context.Rooms.Find(id);
        if (room != null)
        {
            room.Status = "Dirty";
            var task = new CleaningTask { RoomId = room.Id, Status = "Pending" };
            _context.CleaningTasks.Add(task);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}