using SQLite;
using Notes.Models;
using System.Runtime.CompilerServices;

namespace Notes.Data
{
    public class NotesDbContext
    {
        public SQLiteConnection connection;

        public NotesDbContext()
        {
        }

        public void Init()
        {
            string databasePath = Constants.DatabasePath;

            if (connection is not null)
            {
                return;
            }

            connection = new SQLiteConnection(databasePath, Constants.Flags);
            connection.CreateTable<Note>();
        }
    }
}
