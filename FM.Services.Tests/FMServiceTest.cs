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
        public void GetAllPlayersOfTheTeamTest()
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
            _mockTeamRepository.Setup(r=>r.GetTeamByName(teamName)).Returns(Task.FromResult(testTeam));
            //Act
            Task<IEnumerable<PlayerDTO>> result = _fmService.GetAllPlayersOfTheTeam(teamName);
            result.Wait();
            //Assert
            Assert.Equal(result.Result.ToList().Count, (expectedPlayers.Count));

        }
    }
}
