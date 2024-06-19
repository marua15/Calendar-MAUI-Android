using System.IO;
using SQLite;
using Project_2.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Project_2.Services
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _database;
        private static DatabaseService _instance;

        public static DatabaseService Instance => _instance ??= new DatabaseService();

        private DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db3");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<User>();
            _database.CreateTable<EventItem>();
        }

        public User GetUser(string username)
        {
            var user = _database.Table<User>().FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.Events = _database.Table<EventItem>().Where(e => e.UserId == user.Id).ToList();
            }
            return user;
        }

        public bool IsUsernameExists(string username)
        {
            return _database.Table<User>().Any(u => u.Username == username);
        }

        public int SaveUser(User user)
        {
            return _database.Insert(user);
        }

        public int SaveEvent(EventItem item)
        {
            return _database.Insert(item);
        }

        public ObservableCollection<EventItem> GetEventsByUser(int userId)
        {
            var events = _database.Table<EventItem>().Where(e => e.UserId == userId).ToList();
            return new ObservableCollection<EventItem>(events);
        }
    }
}
