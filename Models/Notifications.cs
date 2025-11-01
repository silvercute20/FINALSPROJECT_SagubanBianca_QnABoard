using System;
using System.ComponentModel.DataAnnotations;

namespace Whispeed_BiancaSaguban.Models
{
    public class Notifications
    {
        [Key]
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }

        public User User { get; set; }
    }
}