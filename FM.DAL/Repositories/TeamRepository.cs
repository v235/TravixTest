﻿using System;
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

        public override async Task<EntityTeam> GetByIdAsync(int id)
        {
            return await _context.Teams.Include(p => p.Players).SingleOrDefaultAsync(t=>t.Id==id);
        }

    }
}