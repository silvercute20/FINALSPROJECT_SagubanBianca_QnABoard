using System;
using System.Collections.Generic;

namespace Whispeed_BiancaSaguban.Models
{

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = "User";
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime? DateJoined { get; set; }
        public bool? IsAdmin { get; set; }
        public string? ProfilePhoto { get; set; }
        public int TokenCount { get; set; } = 0;
        public int XP { get; set; } = 0;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }

}
