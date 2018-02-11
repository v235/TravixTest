using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
