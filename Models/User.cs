using SQLite;
using System.Collections.Generic;

namespace Project_2.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Ignore] // This attribute tells SQLite to ignore this property when creating the table
        public List<EventItem> Events { get; set; } = new List<EventItem>();
    }
}
