using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwdProject.Models;
using System.Security.Claims;

namespace SwdProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly SwdProjectContext _context;

        public ProfileController(SwdProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email ?? "",
                FullName = user.FullName ?? "",
                Phone = user.Phone,
                Address = user.Address,
                Avatar = user.Avatar,
                RoleName = user.Role?.Name ?? "Chưa xác định",
                RoleId = user.RoleId
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email ?? "",
                FullName = user.FullName ?? "",
                Phone = user.Phone,
                Address = user.Address,
                Avatar = user.Avatar,
                RoleName = user.Role?.Name ?? "Chưa xác định",
                RoleId = user.RoleId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Kiểm tra email trùng lặp (ngoại trừ user hiện tại)
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == model.Email && u.Id != user.Id);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng bởi người dùng khác");
                return View(model);
            }

            // Cập nhật thông tin user
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.Phone = model.Phone;
            user.Address = model.Address;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Kiểm tra mật khẩu hiện tại
            if (user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng");
                return View(model);
            }

            // Cập nhật mật khẩu mới
            user.Password = model.NewPassword; // TODO: Hash password in production
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Index");
        }
    }
} 