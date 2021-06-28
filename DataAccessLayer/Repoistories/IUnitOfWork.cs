using Entities;
using System.Threading.Tasks;

namespace Repoistories
{
    public interface IUnitOfWork
    {
        public IBookRepository Books { get; }
        public Task Save();
    }
}
