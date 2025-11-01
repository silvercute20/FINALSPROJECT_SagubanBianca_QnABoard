using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Whispeed_BiancaSaguban.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string connectionString =
            "server=localhost;port=3306;database=whispeed_db;user=root;password=;";

        public IActionResult Index()
        {
            int totalUsers = 0;
            int totalPosts = 0;
            var moodCounts = new Dictionary<string, int>
            {
                {"Inlove", 0 },
                {"Sexy", 0 },
                {"Sad", 0 },
                {"Studying", 0 },
                {"Curious", 0 }
            };

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Total Users
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM Users", conn))
                {
                    totalUsers = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Total Posts
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM Questions", conn))
                {
                    totalPosts = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Mood counts
                using (var cmd = new MySqlCommand(
                    "SELECT Mood, COUNT(*) AS Count FROM Questions GROUP BY Mood", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mood = reader["Mood"].ToString();
                            int count = Convert.ToInt32(reader["Count"]);
                            if (moodCounts.ContainsKey(mood))
                            {
                                moodCounts[mood] = count;
                            }
                        }
                    }
                }
            }

            ViewData["TotalUsers"] = totalUsers;
            ViewData["TotalPosts"] = totalPosts;
            ViewData["MoodCounts"] = moodCounts;

            return View();
        }
    }
}
