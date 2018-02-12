using System;
using System.Collections.Generic;
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
        private readonly Mock<ILogger<TeamController>> _mockLogger;
        private readonly TeamController _teamController;

        public TeamControllerTest()
        {
            _mockFMService = new Mock<IFMService>();
            _mockLogger = new Mock<ILogger<TeamController>>();
            _teamController = new TeamController(_mockFMService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Get_Returns_OkResult_and_statusCode_200()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
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
            _mockFMService.Setup(s => s.GetTeams()).ReturnsAsync(expextedTeams);
            //Act
            var result = await _teamController.Get() as OkObjectResult;
            var listOfTeams = result.Value as List<CreateTeamViewModel>;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotEmpty(listOfTeams);
            Assert.Equal(expextedTeams.Count, listOfTeams.Count);
        }

        [Fact]
        public async Task Get_Returns_BadRequest()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            _mockFMService.Setup(s => s.GetTeams()).Throws<Exception>();
            //Act
            var result = await _teamController.Get() as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Post_Returns_CreatedResult_and_statusCode_201()
        {
            //Arrang
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            var expectednewTeam = new CreateTeamViewModel()
            {
                Name = "testTeam"
                
            };
            _mockFMService.Setup(s => s.AddNewTeam(It.IsAny<TeamDTO>())).ReturnsAsync(true);
            //Act
            var result = await _teamController.Post(expectednewTeam) as CreatedResult;
            var newTeam = result.Value as CreateTeamViewModel;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(expectednewTeam.Name, newTeam.Name);
        }
        [Fact]
        public async Task Post_Returns_BadRequest()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            _mockFMService.Setup(s => s.AddNewTeam(It.IsAny<TeamDTO>())).Throws<Exception>();
            //Act
            var result = await _teamController.Post(new CreateTeamViewModel()) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Put_Returns_OkResult_and_statusCode_200()
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
            _mockFMService.Setup(s => s.UpdateTeamValue(It.IsAny<TeamDTO>())).ReturnsAsync(true);
            //Act
            var result = await _teamController.Put(expectednewTeam) as OkObjectResult;
            var newTeam = result.Value as UpdateTeamViewModel;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectednewTeam.Name, newTeam.Name);
        }
        [Fact]
        public async Task Put_Returns_BadRequest()
        {
            //Arrange
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
            });
            _mockFMService.Setup(s => s.UpdateTeamValue(It.IsAny<TeamDTO>())).Throws<Exception>();
            //Act
            var result = await _teamController.Put(new UpdateTeamViewModel()) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
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
            string teamName = "testName";
            _mockFMService.Setup(s => s.DeleteTeam(teamName)).ReturnsAsync(true);
            //Act
            var result = await _teamController.Delete(teamName) as OkResult;
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
            string teamName = "testName";
            _mockFMService.Setup(s => s.DeleteTeam(teamName)).Throws<Exception>();
            //Act
            var result = await _teamController.Delete(teamName) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

    }
}
