using Microsoft.AspNetCore.Mvc;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Controllers;
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show all users
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Show create form
        public IActionResult Create()
        {
            return View();
        }

        // Save user
        [HttpPost]
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
    }
