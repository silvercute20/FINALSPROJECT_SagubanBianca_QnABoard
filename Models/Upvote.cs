using System;

namespace Whispeed_BiancaSaguban.Models
{
    public class Upvote
    {
        public int UpvoteID { get; set; }
        public int AnswerID { get; set; }
        public int UserID { get; set; }
        public DateTime DateVoted { get; set; }

        public Answer Answer { get; set; }
        public User User { get; set; }
    }
}