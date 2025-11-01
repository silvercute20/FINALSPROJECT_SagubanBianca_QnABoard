using Microsoft.AspNetCore.Mvc;
using Whispeed_BiancaSaguban.Data;
using Whispeed_BiancaSaguban.Models;
using System.Linq;

namespace Whispeed_BiancaSaguban.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var notifications = _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.DateCreated)
                .ToList();

            var userPosts = _context.Questions
                .Where(q => q.UserID == userId)
                .OrderByDescending(q => q.DatePosted)
                .ToList();

            ViewBag.YourActivity = userPosts;
            ViewBag.Count = notifications.Count;

            return View(notifications);
        }

        [HttpPost]
        public IActionResult AddNotification(int userId, string message)
        {
            if (userId == 0 || string.IsNullOrWhiteSpace(message))
                return Json(new { success = false });

            var notification = new Notifications
            {
                UserID = userId,
                Message = message.Trim(),
                IsRead = false,
                DateCreated = DateTime.Now
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}