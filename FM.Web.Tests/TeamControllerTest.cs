using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FM.DAL.Models;
using FM.Services;
using FM.Services.Models;
using FM.Web.Controllers;
using FM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FM.Web.Tests
{
    public class TeamControllerTest
    {
        private readonly Mock<IFMService> _mockFMService;
        private readonly TeamController _teamController;

        public TeamControllerTest()
        {
            _mockFMService = new Mock<IFMService>();
            _teamController = new TeamController(_mockFMService.Object);
        }

        [Fact]
        public async Task Get_Returns_statusCode_200()
        {
            //Arrange
            Mapper.Initialize(config => { config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap(); });
            var expextedTeams = new List<TeamDTO>()
            {
                new TeamDTO()
                {
                    Id = 1,
                    Name = "testTeam"
                },
                new TeamDTO()
                {
                    Id = 2,
                    Name = "testTeam2"
                }

            };
            _mockFMService.Setup(s => s.GetTeamsAsync()).ReturnsAsync(expextedTeams);
            //Act
            var result = await _teamController.GetAsync() as OkObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Get_Returns_BadRequest()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            _mockFMService.Setup(s => s.GetTeamsAsync()).ReturnsAsync(Enumerable.Empty<TeamDTO>());
            //Act
            var result = await _teamController.GetAsync() as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Get_Returns_list_of_teams()
        {
            //Arrange
            Mapper.Initialize(config => { config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap(); });
            var expextedTeams = new List<TeamDTO>()
            {
                new TeamDTO()
                {
                    Id = 1,
                    Name = "testTeam"
                },
                new TeamDTO()
                {
                    Id = 2,
                    Name = "testTeam2"
                }

            };
            _mockFMService.Setup(s => s.GetTeamsAsync()).ReturnsAsync(expextedTeams);
            //Act
            var result = await _teamController.GetAsync() as OkObjectResult;
            var listOfTeams = result.Value as List<CreateTeamViewModel>;
            //Assert
            Mapper.Reset();
            Assert.Collection(expextedTeams,
                exp1 => listOfTeams.Any(r => r.Name == exp1.Name),
                exp2 => listOfTeams.Any(r => r.Name == exp2.Name));
        }

        [Fact]
        public async Task Get_by_Id_Returns_statusCode_200()
        {
            //Arrange
            Mapper.Initialize(config => { config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap(); });
            int teamId = 1;
            var expextedTeam = new TeamDTO()
            {
                Id = 1,
                Name = "testTeam"
            };
            _mockFMService.Setup(s => s.GetTeamAsync(teamId)).ReturnsAsync(expextedTeam);
            //Act
            var result = await _teamController.GetAsync(teamId) as OkObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Get_by_Id_Returns_team()
        {
            //Arrange
            Mapper.Initialize(config => { config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap(); });
            int teamId = 1;
            var expextedTeam = new TeamDTO()
            {
                Id = teamId,
                Name = "testTeam"
            };
            _mockFMService.Setup(s => s.GetTeamAsync(teamId)).ReturnsAsync(expextedTeam);
            //Act
            var result = await _teamController.GetAsync(teamId) as OkObjectResult;
            var resultTeam = result.Value as CreateTeamViewModel;
            //Assert
            Mapper.Reset();
            Assert.Equal(expextedTeam.Name, resultTeam.Name);

        }
        [Fact]
        public async Task Get_by_Id_Returns_BadRequest()
        {
            //Arrange
            Mapper.Initialize(config => { config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap(); });
            int teamId = 1;
            TeamDTO expextedTeam = null;
            _mockFMService.Setup(s => s.GetTeamAsync(teamId)).ReturnsAsync(expextedTeam);
            //Act
            var result = await _teamController.GetAsync(teamId) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);

        }
        [Fact]
        public async Task Post_Returns_statusCode_201()
        {
            //Arrang
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            int expectedId = 1;
            var expectednewTeam = new CreateTeamViewModel()
            {
                Name = "testTeam"

            };
            _mockFMService.Setup(s => s.AddNewTeamAsync(It.IsAny<TeamDTO>())).ReturnsAsync(expectedId);
            //Act
            var result = await _teamController.PostAsync(expectednewTeam) as CreatedResult;
            var newTeam = result.Value as CreateTeamViewModel;
            //Assert
            Mapper.Reset();
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task Post_Returns_created_location()
        {
            //Arrang
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            int expectedId = 1;
            string expectedLocation = "api/teams/1";
            var expectednewTeam = new CreateTeamViewModel()
            {
                Name = "testTeam"

            };
            _mockFMService.Setup(s => s.AddNewTeamAsync(It.IsAny<TeamDTO>())).ReturnsAsync(expectedId);
            //Act
            var result = await _teamController.PostAsync(expectednewTeam) as CreatedResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(expectedLocation, result.Location);
        }

        [Fact]
        public async Task Post_Returns_BadRequest()
        {
            //Arrang
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            int expectedId = 1;
            var expectednewTeam = new CreateTeamViewModel()
            {
                Name = "testTeam"

            };
            _mockFMService.Setup(s => s.AddNewTeamAsync(It.IsAny<TeamDTO>())).ReturnsAsync(0);
            //Act
            var result = await _teamController.PostAsync(expectednewTeam) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Put_Returns_statusCode_200()
        {
            //Arrang
            Mapper.Initialize(config =>
            {
                config.CreateMap<UpdateTeamViewModel, TeamDTO>().ReverseMap();
            });
            var expectednewTeam = new UpdateTeamViewModel()
            {
                Name = "testTeam"

            };
            _mockFMService.Setup(s => s.UpdateTeamValueAsync(It.IsAny<TeamDTO>())).ReturnsAsync(true);
            //Act
            var result = await _teamController.PutAsync(expectednewTeam) as OkObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Put_Returns_updatedTeam()
        {
            //Arrang
            Mapper.Initialize(config =>
            {
                config.CreateMap<UpdateTeamViewModel, TeamDTO>().ReverseMap();
            });
            var expectedUpdatedTeam = new UpdateTeamViewModel()
            {
                Id = 2,
                Name = "testTeam"
            };
            _mockFMService.Setup(s => s.UpdateTeamValueAsync(It.IsAny<TeamDTO>())).ReturnsAsync(true);
            //Act
            var result = await _teamController.PutAsync(expectedUpdatedTeam) as OkObjectResult;
            var updatedTeam = result.Value as UpdateTeamViewModel;
            //Assert
            Mapper.Reset();
            Assert.Equal(expectedUpdatedTeam.Id, updatedTeam.Id);
            Assert.Equal(expectedUpdatedTeam.Name, updatedTeam.Name);
        }

        [Fact]
        public async Task Put_Returns_BadRequest()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            _mockFMService.Setup(s => s.UpdateTeamValueAsync(It.IsAny<TeamDTO>())).ReturnsAsync(false);
            //Act
            var result = await _teamController.PutAsync(new UpdateTeamViewModel()) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Delete_Returns_and_statusCode_200()
        {
            //Arrang
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            int teamId = 1;
            _mockFMService.Setup(s => s.DeleteTeamAsync(teamId)).ReturnsAsync(true);
            //Act
            var result = await _teamController.DeleteAsync(teamId) as OkResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Delete_Returns_BadRequest()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            int teamId = 1;
            _mockFMService.Setup(s => s.DeleteTeamAsync(teamId)).ReturnsAsync(false);
            //Act
            var result = await _teamController.DeleteAsync(teamId) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);
        }

    }
}
