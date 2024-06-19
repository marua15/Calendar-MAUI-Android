using SQLite;

namespace Project_2.Models
{
    public class EventItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Date { get; set; }
        public string Event { get; set; }
        public int UserId { get; set; }
    }
}

