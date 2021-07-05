using Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repoistories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public List<TEntity> GetAll(Filter filter);

        public Task<List<TEntity>> GetAllAsync();

        public Task<TEntity> GetById(long id);

        public Task<TEntity> Create(TEntity entity);

        public TEntity Update(TEntity entity);

        public TEntity FirstOrDefalut(Func<TEntity, bool> condition);

        public List<TEntity> Where(Func<TEntity, bool> condition);

        public void Delete(TEntity entity);
    }
}
