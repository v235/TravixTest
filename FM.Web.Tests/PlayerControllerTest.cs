using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly PlayerController _playerController;

        public PlayerControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreatePlayerViewModel, PlayerDTO>().ReverseMap();
            });

            _mockFMService = new Mock<IFMService>();
            _playerController = new PlayerController(_mockFMService.Object);

        }

        [Fact]
        public async Task GetAsync_Returns_statusCode_200()
        {
            //Arrange
            var expextedPlayers = GetExpectedPlayers();
            _mockFMService.Setup(s => s.GetAllPlayersAsync()).ReturnsAsync(expextedPlayers);
            //Act
            var result = await _playerController.Get() as OkObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAsync_Returns_BadRequest()
        {
            //Arrange
            _mockFMService.Setup(s => s.GetAllPlayersAsync()).ReturnsAsync(Enumerable.Empty<PlayerDTO>());
            //Act
            var result = await _playerController.Get() as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task GetAsync_Returns_list_of_players()
        {
            //Arrange
            var expextedPlayers = GetExpectedPlayers();
            _mockFMService.Setup(s => s.GetAllPlayersAsync()).ReturnsAsync(expextedPlayers);
            //Act
            var result = await _playerController.Get() as OkObjectResult;
            var listOfPlayers = result.Value as List<CreatePlayerViewModel>;
            //Assert
            Mapper.Reset();
            Assert.Collection(expextedPlayers,
                exp1 => listOfPlayers.Any(r =>
                    r.Name == exp1.Name && r.Position == exp1.Position && r.Age == exp1.Age),
                exp2 => listOfPlayers.Any(r =>
                    r.Name == exp2.Name && r.Position == exp2.Position && r.Age == exp2.Age));
        }

        [Fact]
        public async Task GetByIdAsync_Returns_statusCode_200()
        {
            //Arrange
            int playerId = 1;
            var expextedPlayer = GetExpectedPlayer();
            _mockFMService.Setup(s => s.GetPlayerAsync(playerId)).ReturnsAsync(expextedPlayer);
            //Act
            var result = await _playerController.GetByIdAsync(playerId) as OkObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_player()
        {
            //Arrange
            int playerId = 1;
            var expextedPlayer = GetExpectedPlayer();
            _mockFMService.Setup(s => s.GetPlayerAsync(playerId)).ReturnsAsync(expextedPlayer);
            //Act
            var result = await _playerController.GetByIdAsync(playerId) as OkObjectResult;
            var resultPlayer = result.Value as CreatePlayerViewModel;
            //Assert
            Mapper.Reset();
            Assert.Equal(expextedPlayer.Name, resultPlayer.Name);
            Assert.Equal(expextedPlayer.Position, resultPlayer.Position);
            Assert.Equal(expextedPlayer.Age, resultPlayer.Age);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_BadRequest()
        {
            //Arrange
            int playerId = 1;
            PlayerDTO expextedPlayer = null;
            _mockFMService.Setup(s => s.GetPlayerAsync(playerId)).ReturnsAsync(expextedPlayer);
            //Act
            var result = await _playerController.GetByIdAsync(playerId) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_Returns_statusCode_201()
        {
            //Arrange
            int expectedId = 1;
            var expectednewPlayer = Mapper.Map<CreatePlayerViewModel>(GetExpectedPlayer());
            _mockFMService.Setup(s => s.AddNewPlayerAsync(It.IsAny<PlayerDTO>())).ReturnsAsync(expectedId);
            //Act
            var result = await _playerController.PostAsync(expectednewPlayer) as CreatedResult;
            var newPlayer = result.Value as CreatePlayerViewModel;
            //Assert
            Mapper.Reset();
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_Returns_BadRequest()
        {
            //Arrange
            _mockFMService.Setup(s => s.AddNewPlayerAsync(It.IsAny<PlayerDTO>())).ReturnsAsync(0);
            //Act
            var result = await _playerController.PostAsync(new CreatePlayerViewModel()) as BadRequestObjectResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_Returns_created_location()
        {
            //Arrange
            int expectedId = 1;
            string expectedLocation = $"api/players/{expectedId}";
            var expectednewPlayer = Mapper.Map<CreatePlayerViewModel>(GetExpectedPlayer());
            _mockFMService.Setup(s => s.AddNewPlayerAsync(It.IsAny<PlayerDTO>())).ReturnsAsync(expectedId);
            //Act
            var result = await _playerController.PostAsync(expectednewPlayer) as CreatedResult;
            //Assert
            Mapper.Reset();
            Assert.Equal(expectedLocation, result.Location);
        }

        private List<PlayerDTO> GetExpectedPlayers()
        {
            return new List<PlayerDTO>()
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
        }

        private PlayerDTO GetExpectedPlayer()
        {
            return new PlayerDTO()
            {
                Id = 1,
                Name = "Test1",
                Position = "TestPos1",
                Age = 25,
                TeamId = 2
            };
        }
    }
}
