using Microsoft.AspNetCore.Mvc;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Controllers;
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show all rooms
        public IActionResult Index()
        {
            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }

        // Show Add Room page
        public IActionResult Create()
        {
            return View();
        }

        // Save new room
        [HttpPost]
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

        // Check-in guest
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

        // Check-out guest
        public IActionResult CheckOut(int id)
        {
            var room = _context.Rooms.Find(id);

            if (room != null)
            {
                room.Status = "Dirty";

                var task = new CleaningTask
                {
                    RoomId = room.Id,
                    Status = "Pending"
                };

                _context.CleaningTasks.Add(task);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
