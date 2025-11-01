using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Whispeed_BiancaSaguban.Data;
using Whispeed_BiancaSaguban.Models;

namespace Whispeed_BiancaSaguban.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Auth");
        }

        [HttpPost]
        public IActionResult Signup(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required";
                return View("Auth");
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email already exists";
                return View("Auth");
            }

            var user = new User
            {
                Username = string.IsNullOrWhiteSpace(username) ? "User" : username,
                Email = email,
                PasswordHash = ComputeHash(password),
                DateJoined = DateTime.Now,
                IsAdmin = true,
                ProfilePhoto = "/images/default-profile.png"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("ProfilePhoto", user.ProfilePhoto);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required";
                return View("Auth");
            }

            
            var hash = ComputeHash(password);
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.PasswordHash == hash);


            if (user != null)
            {
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("Username", user.Username ?? "User");
                HttpContext.Session.SetString("ProfilePhoto", user.ProfilePhoto ?? "/images/default-profile.png");


                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password";
            return View("Auth");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
        [HttpPost]
        public IActionResult UpdateProfile([FromBody] ProfileUpdateModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return Json(new { success = false });

            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null) return Json(new { success = false });

            user.Username = string.IsNullOrWhiteSpace(model.username) ? user.Username : model.username.Trim();
            user.ProfilePhoto = string.IsNullOrWhiteSpace(model.profilePhoto) ? user.ProfilePhoto : model.profilePhoto.Trim();

            _context.SaveChanges();

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("ProfilePhoto", user.ProfilePhoto ?? "/images/default-profile.png");

            return Json(new { success = true, username = user.Username, profilePhoto = user.ProfilePhoto });
        }

        public class ProfileUpdateModel
        {
            public string username { get; set; }
            public string profilePhoto { get; set; }
        }

    }
}
