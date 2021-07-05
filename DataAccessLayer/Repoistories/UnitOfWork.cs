using Data;
using Entities;
using System.Threading.Tasks;

namespace Repoistories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBookRepository _books;

        public IRepository<Author> _authors;

        public IRepository<Publisher> _publisher;

        public IRepository<BookAuthor> _bookAuthors;


        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
        }



        public IBookRepository Books
        {
            get
            {
                return _books ?? (_books = new BookRepository(_context));
            }
        }

        public IRepository<Publisher> Publishers
        {
            get
            {
                return _publisher ?? (_publisher = new BaseRepository<Publisher>(_context));
            }
        }

        public IRepository<Author> Authors
        {
            get
            {
                return _authors ?? (_authors = new BaseRepository<Author>(_context));
            }
        }

        public IRepository<BookAuthor> BookAuthors
        {
            get
            {
                return _bookAuthors ?? (_bookAuthors = new BaseRepository<BookAuthor>(_context));
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
