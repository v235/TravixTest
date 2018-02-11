using FM.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FM.DAL
{
    public class FMContext : DbContext
    {
        public FMContext(DbContextOptions<FMContext> options)
            : base(options)
        {

        }
        public DbSet<EntityPlayer> Players { get; set; }
        public DbSet<EntityTeam> Teams { get; set; }

    }
}