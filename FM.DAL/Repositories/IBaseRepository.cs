using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
