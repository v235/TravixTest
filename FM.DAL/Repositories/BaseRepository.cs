using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<bool> Create(T entity)
        {
            await _entities.AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> Update(T entity)
        {
            _entities.Update(entity);
            return await SaveChanges();
        }
        public async Task<bool> Delete(T entity)
        {
            _entities.Remove(entity);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}