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
            Mapper.Initialize(config =>
            {
                config.CreateMap<EntityPlayer, PlayerDTO>().ReverseMap();
                config.CreateMap<EntityTeam, TeamDTO>().ReverseMap();
            });

            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _fmService = new FMService(_mockTeamRepository.Object, _mockPlayerRepository.Object);
        }

        [Fact]
        public async Task GetAllPlayersOfTheTeamTest_result_notEmpty_collection()
        {

            //Arrange
            string teamName = "testName";
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

            List<PlayerDTO> expectedPlayers = new List<PlayerDTO>()
            {
                new PlayerDTO()
                {
                    Id = 1,
                    Name = "Test1",
                    Position = "TestPos1",
                    Age = 25
                },
                new PlayerDTO()
                {
                    Id = 2,
                    Name = "Test2",
                    Position = "TestPos2",
                    Age = 25
                }
            };
            _mockTeamRepository.Setup(r=>r.GetTeamByName(teamName)).ReturnsAsync(testTeam);
            //Act
            var result = await _fmService.GetAllPlayersOfTheTeam(teamName);
            //Assert
            Mapper.Reset();
            Assert.NotEmpty(result);
            Assert.Equal(expectedPlayers.Count, result.ToList().Count);


        }
        //TODO
        [Fact]
        public async Task AddNewPlayerToTeamTest_result_equal_true()
        {

            //Arrange
            string teamName = "testName";
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
            var newPlayer=new EntityPlayer()
            {
                Id = 3,
                Name = "Test3",
                Position = "TestPos3",
                Age = 25
            };
            var newPlayerDTO = new PlayerDTO()
            {
                Id = 3,
                Name = "Test3",
                Position = "TestPos3",
                Age = 25
            };
            _mockTeamRepository.Setup(r => r.GetTeamByName(teamName)).ReturnsAsync(testTeam);
            _mockPlayerRepository.Setup(r => r.Create(It.IsAny<EntityPlayer>())).ReturnsAsync(true);
            //Act
            var result = await _fmService.AddNewPlayerToTeam(teamName, newPlayerDTO);
            //Assert
            Mapper.Reset();
            Assert.True(result);

        }

        [Fact]
        public async Task GetTeams_result_is_not_empty_collection()
        {
            //Arrange
            List<EntityTeam> expectedTeams = new List<EntityTeam>()
            {
                new EntityTeam()
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
                }
            };

            _mockTeamRepository.Setup(r => r.GetAll()).ReturnsAsync(expectedTeams);
            //Act
            var result = await _fmService.GetTeams();
            //Assert
            Mapper.Reset();
            Assert.NotEmpty(result);
            Assert.Equal(expectedTeams.Count, result.Count());

        }

        [Fact]
        public async Task GetTeam_result_is_not_null()
        {
            //Arrange
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

            _mockTeamRepository.Setup(r => r.GetById(testId)).ReturnsAsync(expectedTeam);
            //Act
            var result = await _fmService.GetTeam(testId);
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.IsType<TeamDTO>(result);

        }

        [Fact]
        public async Task AddNewTeam_result_is_true()
        {
            //Arrange
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
            TeamDTO newTeam = new TeamDTO()
            {
                Id = 1,
                Name = "testTeam",
                Players = new List<PlayerDTO>()
                {
                    new PlayerDTO()
                    {
                        Id = 1,
                        Name = "Test1",
                        Position = "TestPos1",
                        Age = 25
                    },
                    new PlayerDTO()
                    {
                        Id = 2,
                        Name = "Test2",
                        Position = "TestPos2",
                        Age = 25
                    }
                }
            };

            _mockTeamRepository.Setup  (r => r.Create(It.IsAny<EntityTeam>())).ReturnsAsync(true);
            //Act
            var result = await _fmService.AddNewTeam(newTeam);
            //Assert
            Mapper.Reset();
            Assert.True(result);

        }

        [Fact]
        public async Task UpdateTeamValue_result_is_true()
        {
            //Arrange
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
            TeamDTO newTeam = new TeamDTO()
            {
                Id = 1,
                Name = "testTeam",
                Players = new List<PlayerDTO>()
                {
                    new PlayerDTO()
                    {
                        Id = 1,
                        Name = "Test1",
                        Position = "TestPos1",
                        Age = 25
                    },
                    new PlayerDTO()
                    {
                        Id = 2,
                        Name = "Test2",
                        Position = "TestPos2",
                        Age = 25
                    }
                }
            };

            _mockTeamRepository.Setup(r => r.Update(It.IsAny<EntityTeam>())).ReturnsAsync(true);
            //Act
            var result = await _fmService.UpdateTeamValue(newTeam);
            //Assert
            Mapper.Reset();
            Assert.True(result);

        }
        [Fact]
        public async Task DeleteTeamValue_result_is_true()
        {
            //Arrange
            string teamName = "testName";
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
            _mockTeamRepository.Setup(r => r.GetTeamByName(teamName)).ReturnsAsync(expectedTeam);
            _mockTeamRepository.Setup(r => r.Delete(expectedTeam)).ReturnsAsync(true);
            //Act
            var result = await _fmService.DeleteTeam(teamName);
            //Assert
            Mapper.Reset();
            Assert.True(result);

        }
    }
}
