using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Villa> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = _dbContext.Set<Villa>();
        }
        public async Task AddAsync(Villa villa)
        {
            await _dbContext.AddAsync(villa);
        }

        public async Task DeleteAsync(Villa villa)
        {
            _dbContext.Remove(villa);
        }

        public async Task<IEnumerable<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = false)
        {
            IQueryable<Villa> query;
            if (tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);  
            }
            return query.ToList();
        }

        public async Task<Villa?> GetAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = false)
        {
            IQueryable<Villa> query;
            if (tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.FirstOrDefault();
        }

        public async Task UpdateAsync(Villa villa)
        {
            _dbSet.Update(villa);
        }
    }
}
