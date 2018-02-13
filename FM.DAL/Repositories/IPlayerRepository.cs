using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FM.DAL.Models;

namespace FM.DAL.Repositories
{
    public interface IPlayerRepository : IBaseRepository<EntityPlayer>
    {
        Task<EntityPlayer> GetById(int id);
    }
}
