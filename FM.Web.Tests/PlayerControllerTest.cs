using System;
using System.Collections.Generic;
using AutoMapper;
using FM.Services;
using FM.Services.Models;
using FM.Web.Controllers;
using FM.Web.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FM.Web.Tests
{
    public class PlayerControllerTest
    {
        private readonly Mock<IFMService> _mockFMService;
        private readonly Mock<ILogger<PlayerController>> _mockLogger;
        private readonly PlayerController _playerController;

        public PlayerControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreatePlayerViewModel, PlayerDTO>().ReverseMap();
            });

            _mockFMService = new Mock<IFMService>();
            _mockLogger = new Mock<ILogger<PlayerController>>();
            _playerController = new PlayerController(_mockFMService.Object, _mockLogger.Object);

        }

        [Fact]
        public async Task Get_Returns_OkResult_and_statusCode_200()
        {
            //Arrange
            string teamName = "testName";
            var expextedPlayers = new List<PlayerDTO>()
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
            _mockFMService.Setup(s => s.GetAllPlayersOfTheTeam(teamName)).ReturnsAsync(expextedPlayers);
            //Act
            var result = await _playerController.Get(teamName) as OkObjectResult;
            var listOfPlayers = result.Value as List<CreatePlayerViewModel>;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotEmpty(listOfPlayers);
            Assert.Equal(expextedPlayers.Count, listOfPlayers.Count);
        }

        [Fact]
        public async Task Get_Returns_BadRequest()
        {
            //Arrange
            string teamName = "testName";
            _mockFMService.Setup(s => s.GetAllPlayersOfTheTeam(teamName)).Throws<Exception>();
            //Act
            var result = await _playerController.Get(teamName) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Post_Returns_CreatedResult_and_statusCode_201()
        {
            //Arrange
            string teamName = "testName";
            CreatePlayerViewModel expectednewPlayer = new CreatePlayerViewModel()
            {
                Name = "test",
                Position = "test",
                Age = 10
            };
            _mockFMService.Setup(s => s. AddNewPlayerToTeam(teamName, It.IsAny<PlayerDTO>())).ReturnsAsync(true );
            //Act
            var result = await _playerController.Post(teamName, expectednewPlayer) as CreatedResult;
            var newPlayer = result.Value as CreatePlayerViewModel;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(expectednewPlayer.Name, newPlayer.Name);
            Assert.Equal(expectednewPlayer.Position, newPlayer.Position);
            Assert.Equal(expectednewPlayer.Age, newPlayer.Age);
        }
        [Fact]
        public async Task Post_Returns_BadRequest()
        {
            //Arrange
            string teamName = "testName";
            CreatePlayerViewModel newPlayer = new CreatePlayerViewModel()
            {
                Name = "test",
                Position = "test",
                Age = 10
            };
            _mockFMService.Setup(s => s.AddNewPlayerToTeam(teamName, It.IsAny<PlayerDTO>())).Throws<Exception>();
            //Act
            var result = await _playerController.Post(teamName, newPlayer) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
