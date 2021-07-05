namespace Repoistories
{
    using Data;
    using Filters;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        protected DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;

            dbSet = context.Set<TEntity>();
        }

        public virtual List<TEntity> GetAll(Filter filter)
        {
            return dbSet.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(long id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public List<TEntity> Where(Func<TEntity, bool> condition)
        {
            return dbSet.Where(condition).ToList();
        }

        public TEntity Update(TEntity entity)
        {
            dbSet.Update(entity);
            return entity;
        }

        public TEntity FirstOrDefalut(Func<TEntity, bool> condition)
        {
            return dbSet.Where(condition).FirstOrDefault();
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }
    }
}
