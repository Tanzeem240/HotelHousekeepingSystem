using Microsoft.AspNetCore.Mvc;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelHousekeepingSystem.Controllers;
    public class UserController : Controller
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
        
        public IActionResult ClockIn()
        {
            var name = HttpContext.Session.GetString("UserName");
            var user = _context.Users.FirstOrDefault(u => u.Name == name);

            if (user != null)
            {
                if (user.ClockInTime != null && user.ClockOutTime == null)
                {
                    return RedirectToAction("Index", "Room");
                }
                
                user.ClockInTime = DateTime.UtcNow;
                user.ClockOutTime = null; // reset previous session
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
                {
                    // cannot clock out without clocking in
                    return RedirectToAction("Index", "Room");
                }

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
            {
                var hours = (user.ClockOutTime - user.ClockInTime)?.TotalHours;

                ViewBag.WorkHours = Math.Round(hours ?? 0, 2);
            }
            else
            {
                ViewBag.WorkHours = 0;
            }

            return View();
        }
    }
