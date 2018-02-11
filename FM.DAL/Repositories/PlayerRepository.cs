using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FM.DAL.Models;

namespace FM.DAL.Repositories
{
    public class PlayerRepository : BaseRepository<EntityPlayer>, IPlayerRepository
    {
        private readonly FMContext _context;

        public PlayerRepository(FMContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
