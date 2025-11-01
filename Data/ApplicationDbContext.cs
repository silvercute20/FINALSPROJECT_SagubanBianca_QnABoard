using Microsoft.EntityFrameworkCore;
using Whispeed_BiancaSaguban.Models;

namespace Whispeed_BiancaSaguban.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Upvote> Upvotes { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        public DbSet<AnswerUpvote> AnswerUpvotes { get; set; }


    }


}