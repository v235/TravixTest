using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FM.DAL.Models;

namespace FM.DAL.Repositories
{
    public interface ITeamRepository : IBaseRepository<EntityTeam>
    {
        Task<IEnumerable<EntityTeam>> GetAllAsync();
        Task<EntityTeam> GetByIdAsync(int id);
    }
}
