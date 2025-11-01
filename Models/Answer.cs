using System;

namespace Whispeed_BiancaSaguban.Models
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime DatePosted { get; set; }
        public int UpvoteCount { get; set; }
        public int ReportCount { get; set; }
        public bool IsPinned { get; set; } = false;
        public bool IsBestAnswer { get; set; } = false;

        public User User { get; set; }
        public Question Question { get; set; }
    }
}
