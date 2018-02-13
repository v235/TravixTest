using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FM.DAL.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace FM.DAL
{
    public class FMContextSeedData
    {
        private readonly FMContext _context;
        public FMContextSeedData(FMContext context)
        {
            _context = context;
        }
        public async Task EnsureSeedData()
        {
                if (!_context.Teams.Any())
                {
                    var barcelona = new EntityTeam()
                    {
                        Name = "Barcelona",
                        Players = new List<EntityPlayer>()
                        {
                            new EntityPlayer()
                            {
                                Name = "Messi",
                                Position = "Striker",
                                Age = 30
                            },
                            new EntityPlayer()
                            {
                                Name = "Ter Stegen",
                                Position = "Goalkeeper",
                                Age = 25
                            },
                            new EntityPlayer()
                            {
                                Name = "Iniesta",
                                Position = "Midfielder",
                                Age = 33
                            }
                        }
                    };
                    _context.Add(barcelona);
                    _context.AddRange(barcelona.Players);
                    var mu = new EntityTeam()
                    {
                        Name = "Manchester United ",
                        Players = new List<EntityPlayer>()
                        {
                            new EntityPlayer()
                            {
                                Name = "Ibrahimovic",
                                Position = "Striker",
                                Age = 35
                            },
                            new EntityPlayer()
                            {
                                Name = "Martial",
                                Position = "Striker",
                                Age = 21
                            },
                            new EntityPlayer()
                            {
                                Name = "Rojo",
                                Position = "Defender",
                                Age = 27
                            }
                        }
                    };
                    _context.Add(mu);
                    _context.AddRange(mu.Players);
                    var real = new EntityTeam()
                    {
                        Name = "Real Madrid",
                        Players = new List<EntityPlayer>()
                        {
                            new EntityPlayer()
                            {
                                Name = "Modric",
                                Position = "Midfielder",
                                Age = 31
                            },
                            new EntityPlayer()
                            {
                                Name = "Ronaldo",
                                Position = "Striker",
                                Age = 32
                            },
                            new EntityPlayer()
                            {
                                Name = "Benzema",
                                Position = "Striker",
                                Age = 29
                            }
                        }
                    };
                    _context.Add(real);
                    _context.AddRange(real.Players);

                    await _context.SaveChangesAsync();
                }
        }
    }
}