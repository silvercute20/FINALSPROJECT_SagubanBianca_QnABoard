using Microsoft.AspNetCore.Mvc;
using Whispeed_BiancaSaguban.Data;
using Whispeed_BiancaSaguban.Models;
using System;
using System.Linq;

namespace Whispeed_BiancaSaguban.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AnswerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddAnswer(int questionId, string content, int isAnonymous)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null || string.IsNullOrWhiteSpace(content))
                return BadRequest("Invalid data");

            var answer = new Answer
            {
                QuestionID = questionId,
                UserID = userId.Value,
                Content = content.Trim(),
                IsAnonymous = isAnonymous == 1,
                DatePosted = DateTime.Now,
                IsBestAnswer = false
            };

            _context.Answers.Add(answer);

            // Add XP for posting answer
            var poster = _context.Users.FirstOrDefault(u => u.UserID == userId.Value);
            if (poster != null)
            {
                poster.XP += 1;
            }

            _context.SaveChanges();

            var displayName = answer.IsAnonymous ? "Anonymous" : poster.Username;
            var displayPhoto = answer.IsAnonymous ? "/images/default-profile.png" : (poster.ProfilePhoto ?? "/images/default-profile.png");

            string html = $@"
<div class='answer-container {(answer.IsBestAnswer ? "best-answer" : "")}' data-id='{answer.AnswerID}' data-userid='{answer.UserID}'>
    <div class='answer-header'>
        <img src='{displayPhoto}' width='30' height='30' />
        <strong>{displayName}</strong> • <small>{answer.DatePosted:g}</small>
    </div>
    <div class='answer-content'>
        <p>{System.Net.WebUtility.HtmlEncode(answer.Content)}</p>
    </div>
    <div class='answer-actions'>
        <span class='select-best-answer' role='button'>✔ Select as Best Answer</span>
    </div>
</div>";

            return Content(html);
        }

        [HttpPost]
        public IActionResult DeleteAnswer(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return Json(new { success = false });

            var answer = _context.Answers.FirstOrDefault(a => a.AnswerID == id && a.UserID == userId);
            if (answer == null)
                return Json(new { success = false });

            _context.Answers.Remove(answer);
            _context.SaveChanges();

            return Json(new { success = true });
        }
        [HttpPost]
        public IActionResult SelectBestAnswer(int answerId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return Json(new { success = false });

            var answer = _context.Answers.FirstOrDefault(a => a.AnswerID == answerId);
            if (answer == null)
                return Json(new { success = false });

            var previousBest = _context.Answers.FirstOrDefault(a => a.QuestionID == answer.QuestionID && a.IsBestAnswer);
            if (previousBest != null)
                previousBest.IsBestAnswer = false;

            answer.IsBestAnswer = true;

            var receiver = _context.Users.FirstOrDefault(u => u.UserID == answer.UserID);
            if (receiver != null)
            {
                receiver.TokenCount += 1;
                receiver.XP += 1;
            }

            _context.SaveChanges();

            return Json(new
            {
                success = true,
                answerId = answer.AnswerID,
                tokenCount = receiver?.TokenCount ?? 0,
                xpCount = receiver?.XP ?? 0
            });
        }


    }
}
