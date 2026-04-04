using Microsoft.AspNetCore.Mvc;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    // LOGIN PAGE
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string name)
    {
        var user = _context.Users.FirstOrDefault(u => u.Name == name);

        if (user != null)
        {
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Index", "Room");
        }

        ViewBag.Error = "User not found";
        return View();
    }

    // REGISTER PAGE
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        return View(user);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}