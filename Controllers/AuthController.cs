using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HotelHousekeepingSystem.Data;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Controllers;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireRoleAttribute : ActionFilterAttribute
{
    private readonly string[] _roles;
    public RequireRoleAttribute() : this(Array.Empty<string>()) { }
    public RequireRoleAttribute(params string[] roles) { _roles = roles; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userName = context.HttpContext.Session.GetString("UserName");
        var userRole = context.HttpContext.Session.GetString("UserRole");

        if (string.IsNullOrEmpty(userName))
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        if (_roles.Length > 0)
        {
            bool hasRole = _roles.Any(r =>
                string.Equals(r, userRole, StringComparison.OrdinalIgnoreCase));
            if (!hasRole)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                return;
            }
        }

        base.OnActionExecuting(context);
    }
}

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Auth/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Auth/Login
    [HttpPost]
    public IActionResult Login(string name)
    {
        var user = _context.Users.FirstOrDefault(u => u.Name == name);

        if (user != null)
        {
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetInt32("UserId", user.Id);
            return RedirectToAction("Index", "Room");
        }

        ViewBag.Error = "User not found";
        return View();
    }

    // GET: /Auth/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Auth/Register — saves User + WorkerProfile together
    [HttpPost]
    public IActionResult Register(string Name, string Role,
                                  string Nationality, string PhoneNumber, string Email)
    {
        // Check duplicate name
        bool exists = _context.Users.Any(u => u.Name == Name);
        if (exists)
        {
            ViewBag.Error = "A user with that name already exists.";
            return View();
        }

        // Save user
        var user = new User { Name = Name, Role = Role };
        _context.Users.Add(user);
        _context.SaveChanges(); // saves so user.Id is populated

        // Save profile
        var profile = new WorkerProfile
        {
            UserId      = user.Id,
            Nationality = Nationality,
            PhoneNumber = PhoneNumber,
            Email       = Email
        };
        _context.WorkerProfiles.Add(profile);
        _context.SaveChanges();

        TempData["Success"] = "Account created! Please log in.";
        return RedirectToAction("Login");
    }

    // GET: /Auth/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    // GET: /Auth/AccessDenied
    public IActionResult AccessDenied()
    {
        return View();
    }
}
