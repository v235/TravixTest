using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FM.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FM.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly FMContext _context;
        private readonly DbSet<T> _entities;
        public BaseRepository(FMContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _entities.AddAsync(entity);
            if (await SaveChangesAsync())
                return entity;
            return null;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            return await SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(T entity)
        {
            _entities.Attach(entity);
            _entities.Remove(entity);
            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}