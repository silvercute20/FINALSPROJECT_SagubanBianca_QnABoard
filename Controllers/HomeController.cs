using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whispeed_BiancaSaguban.Data;
using Whispeed_BiancaSaguban.Models;
using System.Linq;

namespace Whispeed_BiancaSaguban.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            var posts = _context.Questions
                .Include(q => q.User)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.User)
                .OrderByDescending(q => q.DatePosted)
                .ToList();

            ViewData["UserID"] = userId;
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfilePhoto") ?? "/images/default-profile.png";

            if (userId != null)
            {
                var user = _context.Users.Find(userId.Value);
                ViewData["TokenCount"] = user?.TokenCount ?? 0;
                ViewData["XP"] = user?.XP ?? 0;
            }
            else
            {
                ViewData["TokenCount"] = 0;
                ViewData["XP"] = 0;
            }

            return View(posts);
        }

        [HttpPost]
        public IActionResult AddQuestion(string title, string mood, int isAnonymous)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var username = HttpContext.Session.GetString("Username");
            if (userId == null || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(mood))
                return BadRequest("Invalid data");

            title = title.Trim();

            var question = new Question
            {
                Title = title,
                Mood = mood,
                IsAnonymous = isAnonymous == 1,
                UserID = userId.Value,
                DatePosted = System.DateTime.Now
            };

            _context.Questions.Add(question);
            _context.SaveChanges();

            var user = _context.Users.Find(userId.Value);
            var displayName = question.IsAnonymous ? "Anonymous" : user.Username;
            var displayPhoto = question.IsAnonymous ? "/images/default-profile.png" : (user.ProfilePhoto ?? "/images/default-profile.png");
            var currentUser = (!question.IsAnonymous && username == user.Username) ? " (You)" : "";

            var moodText = question.Mood switch
            {
                "Inlove" => "is feeling inlove \u2764",
                "Sexy" => "is feeling sexy \uD83D\uDD25",
                "Sad" => "is feeling sad \uD83D\uDE22",
                "Studying" => "is studying \uD83D\uDCDA",
                "Curious" => "is curious \uD83E\uDD14",
                _ => ""
            };

            var bgColor = question.Mood switch
            {
                "Inlove" => "#F09367",
                "Sexy" => "#FF0000",
                "Sad" => "#FFBE98",
                "Studying" => "#F8D8D4",
                "Curious" => "#CE5A43",
                _ => "#F8D8D4"
            };

            string html = $@"
            <div class='post-container' style='background-color:{bgColor}' data-id='{question.QuestionID}'>
                <div class='post-header'>
                    <div class='post-header-left'>
                        <img src='{displayPhoto}' width='40' height='40' />
                        <div class='post-header-meta'>
                            <strong>{System.Net.WebUtility.HtmlEncode(displayName)}{currentUser}</strong>
                            <small class='mood-text'>{System.Net.WebUtility.HtmlEncode(moodText)}</small>
                        </div>
                    </div>
                    <div class='post-header-right'>
                        <small>{question.DatePosted:g}</small>
                    </div>
                </div>
                <div class='post-content'>
                    <p>{System.Net.WebUtility.HtmlEncode(question.Title)}</p>
                </div>
            </div>";

            return Content(html);
        }

        [HttpPost]
        public IActionResult UpdateQuestion(int id, string newTitle)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return BadRequest(new { success = false });
            var question = _context.Questions.FirstOrDefault(q => q.QuestionID == id && q.UserID == userId);
            if (question == null) return BadRequest(new { success = false });
            question.Title = newTitle.Trim();
            _context.SaveChanges();
            return Ok(new { success = true, updatedTitle = question.Title });
        }

        [HttpPost]
        public IActionResult DeleteQuestion(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return Json(new { success = false });

            var question = _context.Questions.Find(id);
            if (question == null || question.UserID != userId) return Json(new { success = false });

            _context.Questions.Remove(question);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdateUserStats(int xp, int tokens)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId != null)
            {
                var user = _context.Users.Find(userId.Value);
                if (user != null)
                {
                    user.XP = xp;
                    user.TokenCount = tokens;
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }
    }
}
