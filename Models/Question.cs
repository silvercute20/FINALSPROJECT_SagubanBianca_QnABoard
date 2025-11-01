using System;
using System.Collections.Generic;

namespace Whispeed_BiancaSaguban.Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string Title { get; set; }
        public string Mood { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsAnonymous { get; set; }
        public int UserID { get; set; }
        public int? BestAnswerID { get; set; }


        public virtual User User { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    }
}
