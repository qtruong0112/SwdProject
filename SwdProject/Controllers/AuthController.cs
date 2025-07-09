using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SwdProject.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SwdProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly SwdProjectContext _context;  

        public AuthController(SwdProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("FullName", user.FullName ?? ""),
                new Claim(ClaimTypes.Role, user.RoleId?.ToString() ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Chuyển hướng về trang mong muốn
            return Redirect("https://localhost:7288/");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng");
                return View(model);
            }

            // Tạo user mới
            var user = new User
            {
                Email = model.Email,
                Password = model.Password, // TODO: Hash password in production
                FullName = model.FullName,
                Phone = model.Phone,
                Address = model.Address,
                RoleId = 1 // Default role (assuming 2 is customer role)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Tự động đăng nhập sau khi đăng ký thành công
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("FullName", user.FullName ?? user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.RoleId?.ToString() ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            TempData["SuccessMessage"] = "Đăng ký thành công! Chào mừng bạn đến với hệ thống.";
            return RedirectToAction("Index", "Home");
        }
    }
}