using Entities;
using System.Threading.Tasks;

namespace Repoistories
{
    public interface IUnitOfWork
    {
        public IBookRepository Books { get; }
        public IRepository<Publisher> Publishers { get; }
        public IRepository<Author> Authors { get; }
        public Task Save();
    }
}
