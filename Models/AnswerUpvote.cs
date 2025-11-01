using System;
using System.ComponentModel.DataAnnotations;

namespace Whispeed_BiancaSaguban.Models
{
    public class AnswerUpvote
    {
        [Key]
        public int AnswerUpvoteID { get; set; } 
        public int AnswerID { get; set; }
        public int UserID { get; set; }
        public DateTime DateVoted { get; set; } = DateTime.Now;

        public Answer Answer { get; set; }
        public User User { get; set; }
    }
}
