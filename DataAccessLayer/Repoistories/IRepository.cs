using Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Repoistories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll(Filter filter);

        Task<TEntity> GetById(long id);

        Task<TEntity> Create(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity FirstOrDefalut(Func<TEntity, bool> condition);

        void Delete(TEntity entity);
    }
}
