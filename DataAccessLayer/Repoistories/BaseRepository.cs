﻿namespace Repoistories
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
        protected readonly ApplicationDbContext _context;

        protected DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual List<TEntity> GetAll(Filter filter)
        {
            return this.dbSet.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
        }

        public List<TEntity> GetAll()
        {
            return this.dbSet.ToList();
        }

        public async Task<TEntity> GetById(long id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await this.dbSet.AddAsync(entity);
            return entity;
        }

        public List<TEntity> Where(Func<TEntity, bool> condition)
        {
            return this.dbSet.Where(condition).ToList();
        }

        public TEntity Update(TEntity entity)
        {
            this.dbSet.Update(entity);
            return entity;
        }

        public TEntity FirstOrDefalut(Func<TEntity, bool> condition)
        {
            return this.dbSet.Where(condition).FirstOrDefault();
        }

        public void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }
    }
}
