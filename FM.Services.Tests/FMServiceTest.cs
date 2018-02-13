using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FM.DAL.Models;
using FM.DAL.Repositories;
using FM.Services.Models;
using Moq;
using Xunit;

namespace FM.Services.Tests
{
    public class FMServiceTest
    {
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly FMService _fmService;

        public FMServiceTest()
        {
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _fmService = new FMService(_mockTeamRepository.Object, _mockPlayerRepository.Object);
        }

        [Fact]
        public async Task GetAllPlayersOfTheTeamAsyncTest_return_collection_of_players()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<TeamDTO, EntityTeam>();
            });
            int teamId = 1;
            EntityTeam testTeam = new EntityTeam()
            {
                Id = 1,
                Name = "testTeam",
                Players = new List<EntityPlayer>()
                {
                    new EntityPlayer()
                    {
                        Id = 1,
                        Name = "Test1",
                        Position = "TestPos1",
                        Age = 25
                    },
                    new EntityPlayer()
                    {
                        Id = 2,
                        Name = "Test2",
                        Position = "TestPos2",
                        Age = 25
                    }
                }
            };
            _mockTeamRepository.Setup(r => r.GetByIdAsync(teamId)).ReturnsAsync(testTeam);
            //Act
            var result = await _fmService.GetAllPlayersOfTheTeamAsync(teamId);
            //Assert
            Mapper.Reset();
            Assert.Collection(testTeam.Players,
                exp1 => result.Any(r => r.Id == exp1.Id && r.Name == exp1.Name 
                        && r.Age==exp1.Age && r.Position==exp1.Position),
                exp2 => result.Any(r => r.Id == exp2.Id && r.Name == exp2.Name
                        && r.Age == exp2.Age && r.Position == exp2.Position));
        }

        [Fact]
        public async Task GetPlayerAsyncTest_return_player_by_id()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<PlayerDTO, EntityPlayer>();

            });
            //Arrange
            int playerId = 1;
            var expectedPlayer = new EntityPlayer()
            {
                Id = 1,
                Name = "Test1",
                Position = "TestPos1",
                Age = 25
            };

            Mapper.Map(expectedPlayer, expectedPlayer);
            _mockPlayerRepository.Setup(r => r.GetByIdAsync(playerId)).ReturnsAsync(expectedPlayer);
            //Act
            var result = await _fmService.GetPlayerAsync(playerId);
            //Assert
            Mapper.Reset();
            Assert.Equal(expectedPlayer.Id, result.Id);
            Assert.Equal(expectedPlayer.Name, result.Name);
            Assert.Equal(expectedPlayer.Position, result.Position);
            Assert.Equal(expectedPlayer.Age, result.Age);
        }

        [Fact]
        public async Task AddNewPlayerToTeamAsyncTest_returns_Id_more_then_zero_wich_is_equal_created()
        {

            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<EntityPlayer, PlayerDTO>().ReverseMap();
            });
            int expectedteamId = 1;
            var createdTestPlayer = new EntityPlayer()
            {
                Id = expectedteamId,
                Name = "test",
                Position = "testPos",
                Age = 20
            };
            _mockPlayerRepository.Setup(r => r.CreateAsync(It.IsAny<EntityPlayer>())).ReturnsAsync(createdTestPlayer);
            //Act
            var result = await _fmService.AddNewPlayerAsync(new PlayerDTO());
            //Assert
            Mapper.Reset();
            Assert.Equal(expectedteamId, result);

        }

        [Fact]
        public async Task AddNewPlayerToTeamAsyncTest_returns_Id_less_then_zero_wich_is_equal_not_created()
        {

            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<EntityPlayer, PlayerDTO>().ReverseMap();
            });
            EntityPlayer createdTestPlayer = null;
            _mockPlayerRepository.Setup(r => r.CreateAsync(It.IsAny<EntityPlayer>())).ReturnsAsync(createdTestPlayer);
            //Act
            var result = await _fmService.AddNewPlayerAsync(new PlayerDTO());
            //Assert
            Mapper.Reset();
            Assert.Equal(0,result);

        }

        [Fact]
        public async Task GetTeamsAsync_returns_coolection_of_the_teams()
        {
            //Arrange
            Mapper.Initialize(config => { config.CreateMap<TeamDTO, EntityTeam>(); });
            List<EntityTeam> expectedTeams = new List<EntityTeam>()
            {
                new EntityTeam()
                {
                    Id = 1,
                    Name = "testTeam1"
                },
                new EntityTeam()
                {
                    Id = 2,
                    Name = "testTeam2"
                }
            };

            _mockTeamRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedTeams);
            //Act
            var result = await _fmService.GetTeamsAsync();
            //Assert
            Mapper.Reset();
            Assert.Collection(expectedTeams,
                exp1 => result.Any(r => r.Id == exp1.Id && r.Name == exp1.Name),
                exp2 => result.Any(r => r.Id == exp2.Id && r.Name == exp2.Name));
        }

        [Fact]
        public async Task GetTeamAsync_returns_is_team_type_of_TeamDTO()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<TeamDTO, EntityTeam>();
            });
            int testId = 1;
            EntityTeam expectedTeam = new EntityTeam()
            {
                Id = 1,
                Name = "testTeam",
                Players = new List<EntityPlayer>()
                    {
                        new EntityPlayer()
                        {
                            Id = 1,
                            Name = "Test1",
                            Position = "TestPos1",
                            Age = 25
                        },
                        new EntityPlayer()
                        {
                            Id = 2,
                            Name = "Test2",
                            Position = "TestPos2",
                            Age = 25
                        }
                    }
            };

            _mockTeamRepository.Setup(r => r.GetByIdAsync(testId)).ReturnsAsync(expectedTeam);
            //Act
            var result = await _fmService.GetTeamAsync(testId);
            //Assert
            Mapper.Reset();
            Assert.Equal(expectedTeam.Id, result.Id);
            Assert.Equal(expectedTeam.Name, result.Name);

        }

        //[Fact]
        //public async Task AddNewTeamAsync_returns_true_wich_is_equal_created()
        //{
        //    //Arrange
        //    Mapper.Initialize(config =>
        //    {
        //        config.CreateMap<EntityTeam, TeamDTO>();
        //    });
        //    TeamDTO newTeam = new TeamDTO()
        //    {
        //        Id = 1,
        //        Name = "testTeam",
        //        Players = new List<PlayerDTO>()
        //        {
        //            new PlayerDTO()
        //            {
        //                Id = 1,
        //                Name = "Test1",
        //                Position = "TestPos1",
        //                Age = 25
        //            },
        //            new PlayerDTO()
        //            {
        //                Id = 2,
        //                Name = "Test2",
        //                Position = "TestPos2",
        //                Age = 25
        //            }
        //        }
        //    };

        //    _mockTeamRepository.Setup(r => r.Create(It.IsAny<EntityTeam>())).ReturnsAsync(true);
        //    //Act
        //    var result = await _fmService.AddNewTeam(newTeam);
        //    //Assert
        //    Mapper.Reset();
        //    Assert.True(result);

        //}
        //[Fact]
        //public async Task AddNewTeamAsync_returns_false_wich_is_equal_not_created()
        //{
        //    //Arrange
        //    Mapper.Initialize(config =>
        //    {
        //        config.CreateMap<EntityTeam, TeamDTO>();
        //    });
        //    TeamDTO newTeam = new TeamDTO()
        //    {
        //        Id = 1,
        //        Name = "testTeam",
        //        Players = new List<PlayerDTO>()
        //        {
        //            new PlayerDTO()
        //            {
        //                Id = 1,
        //                Name = "Test1",
        //                Position = "TestPos1",
        //                Age = 25
        //            },
        //            new PlayerDTO()
        //            {
        //                Id = 2,
        //                Name = "Test2",
        //                Position = "TestPos2",
        //                Age = 25
        //            }
        //        }
        //    };

        //    _mockTeamRepository.Setup(r => r.Create(It.IsAny<EntityTeam>())).ReturnsAsync(false);
        //    //Act
        //    var result = await _fmService.AddNewTeam(newTeam);
        //    //Assert
        //    Mapper.Reset();
        //    Assert.False(result);

        //}

        //[Fact]
        //public async Task UpdateTeamValueAsync_returns_true_wich_is_equal_updated()
        //{
        //    //Arrange
        //    Mapper.Initialize(config =>
        //    {
        //        config.CreateMap<EntityTeam, TeamDTO>();
        //    });
        //    TeamDTO newTeam = new TeamDTO()
        //    {
        //        Id = 1,
        //        Name = "testTeam",
        //        Players = new List<PlayerDTO>()
        //        {
        //            new PlayerDTO()
        //            {
        //                Id = 1,
        //                Name = "Test1",
        //                Position = "TestPos1",
        //                Age = 25
        //            },
        //            new PlayerDTO()
        //            {
        //                Id = 2,
        //                Name = "Test2",
        //                Position = "TestPos2",
        //                Age = 25
        //            }
        //        }
        //    };

        //    _mockTeamRepository.Setup(r => r.Update(It.IsAny<EntityTeam>())).ReturnsAsync(true);
        //    //Act
        //    var result = await _fmService.UpdateTeamValue(newTeam);
        //    //Assert
        //    Mapper.Reset();
        //    Assert.True(result);

        //}

        //[Fact]
        //public async Task UpdateTeamValueAsync_returns_false_wich_is_equal_not_updated()
        //{
        //    //Arrange
        //    Mapper.Initialize(config =>
        //    {
        //        config.CreateMap<EntityTeam, TeamDTO>();
        //    });
        //    TeamDTO newTeam = new TeamDTO()
        //    {
        //        Id = 1,
        //        Name = "testTeam",
        //        Players = new List<PlayerDTO>()
        //        {
        //            new PlayerDTO()
        //            {
        //                Id = 1,
        //                Name = "Test1",
        //                Position = "TestPos1",
        //                Age = 25
        //            },
        //            new PlayerDTO()
        //            {
        //                Id = 2,
        //                Name = "Test2",
        //                Position = "TestPos2",
        //                Age = 25
        //            }
        //        }
        //    };

        //    _mockTeamRepository.Setup(r => r.Update(It.IsAny<EntityTeam>())).ReturnsAsync(false);
        //    //Act
        //    var result = await _fmService.UpdateTeamValue(newTeam);
        //    //Assert
        //    Mapper.Reset();
        //    Assert.False(result);

        //}

        //[Fact]
        //public async Task DeleteTeamValueAsync_returns_true_wich_is_equal_deleteded()
        //{
        //    //Arrange
        //    Mapper.Initialize(config =>
        //    {
        //        config.CreateMap<EntityTeam, TeamDTO>();
        //    });
        //    string teamName = "testName";
        //    EntityTeam expectedTeam = new EntityTeam()
        //    {
        //        Id = 1,
        //        Name = "testTeam",
        //        Players = new List<EntityPlayer>()
        //        {
        //            new EntityPlayer()
        //            {
        //                Id = 1,
        //                Name = "Test1",
        //                Position = "TestPos1",
        //                Age = 25
        //            },
        //            new EntityPlayer()
        //            {
        //                Id = 2,
        //                Name = "Test2",
        //                Position = "TestPos2",
        //                Age = 25
        //            }
        //        }
        //    };
        //    _mockTeamRepository.Setup(r => r.GetTeamByName(teamName)).ReturnsAsync(expectedTeam);
        //    _mockTeamRepository.Setup(r => r.Delete(expectedTeam)).ReturnsAsync(true);
        //    //Act
        //    var result = await _fmService.DeleteTeam(teamName);
        //    //Assert
        //    Mapper.Reset();
        //    Assert.True(result);

        //}
        //[Fact]
        //public async Task DeleteTeamValueAsync_returns_false_wich_is_equal_not_deleteded()
        //{
        //    //Arrange
        //    Mapper.Initialize(config =>
        //    {
        //        config.CreateMap<EntityTeam, TeamDTO>();
        //    });
        //    string teamName = "testName";
        //    EntityTeam expectedTeam = new EntityTeam()
        //    {
        //        Id = 1,
        //        Name = "testTeam",
        //        Players = new List<EntityPlayer>()
        //        {
        //            new EntityPlayer()
        //            {
        //                Id = 1,
        //                Name = "Test1",
        //                Position = "TestPos1",
        //                Age = 25
        //            },
        //            new EntityPlayer()
        //            {
        //                Id = 2,
        //                Name = "Test2",
        //                Position = "TestPos2",
        //                Age = 25
        //            }
        //        }
        //    };
        //    _mockTeamRepository.Setup(r => r.GetTeamByName(teamName)).ReturnsAsync(expectedTeam);
        //    _mockTeamRepository.Setup(r => r.Delete(expectedTeam)).ReturnsAsync(false);
        //    //Act
        //    var result = await _fmService.DeleteTeam(teamName);
        //    //Assert
        //    Mapper.Reset();
        //    Assert.False(result);

        //}
    }
}
