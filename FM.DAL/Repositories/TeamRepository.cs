using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FM.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FM.DAL.Repositories
{
    public class TeamRepository : BaseRepository<EntityTeam>, ITeamRepository
    {
        private readonly FMContext _context;

        public TeamRepository(FMContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EntityTeam>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<EntityTeam> GetById(int id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task<EntityTeam> GetTeamByName(string name)
        {
            return await _context.Teams.Include(p => p.Players)
                .SingleOrDefaultAsync(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}