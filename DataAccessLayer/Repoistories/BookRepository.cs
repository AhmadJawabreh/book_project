﻿using Data;
using Entities;
using Filters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Repoistories
{

    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> GetBookWithAuthors(long id);
    }

    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override List<Book> GetAll(Filter filter)
        {

            IQueryable<Book> _dbSet = dbSet;

            _dbSet = dbSet.Include(item => item.BookAuthors);

            if (!string.IsNullOrEmpty(filter.BookName))
            {
                _dbSet = _dbSet.Where(item => item.Name == filter.BookName);
            }

            List<Book> books = _dbSet.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();

            return books;
        }

        public async Task<Book> GetBookWithAuthors(long Id)
        {
            IQueryable<Book> _dbSet = dbSet;

            _dbSet = dbSet.Include(item => item.BookAuthors);

            return await _dbSet.FirstOrDefaultAsync(item => item.Id == Id);
        }
    }
}
