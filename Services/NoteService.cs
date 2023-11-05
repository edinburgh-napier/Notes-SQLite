using Notes.Models;
using Notes.Data;

namespace Notes.Services
{
    class NoteService : INoteService
    {
        public NotesDbContext _dbContext;
        public NoteService(NotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int DeleteItem(Note item)
        {
            _dbContext.Init();
            return _dbContext.connection.Delete(item);
        }

        public List<Note> GetItems()
        {
            _dbContext.Init();
            return _dbContext.connection.Table<Note>().ToList();
        }

        public Note GetItem(int id)
        {
            _dbContext.Init();
            return  _dbContext.connection.Table<Note>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveItem(Note item)
        {
            _dbContext.Init();
            if (item.Id != 0)
                return _dbContext.connection.Update(item);
            else
                return _dbContext.connection.Insert(item);
        }

    }
}
