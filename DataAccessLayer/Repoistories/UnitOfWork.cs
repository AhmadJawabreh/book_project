using Data;
using Entities;
using System.Threading.Tasks;

namespace Repoistories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;


        public IBookRepository _books;


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

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
