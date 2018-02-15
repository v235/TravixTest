using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; 
using FM.Services;
using FM.Services.Models;
using FM.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FM.Web.Controllers
{
    [Route("api/players")]
    public class PlayerController : Controller
    {
        private readonly IFMService _fmService;
        public PlayerController(IFMService fmService)
        {
            _fmService = fmService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var players = Mapper.Map<IEnumerable<CreatePlayerViewModel>>
                (await _fmService.GetAllPlayersAsync());
            if (players.Any())
                return Ok(players);
            return BadRequest("Failed to get players");
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetByIdAsync(int playerId)
        {
            var player = Mapper.Map<CreatePlayerViewModel>
                (await _fmService.GetPlayerAsync(playerId));
            if (player != null)
                return Ok(player);
            return BadRequest("Failed to get player"); 
        }

        [HttpPost("player")]
        public async Task<IActionResult> PostAsync([FromBody] CreatePlayerViewModel player)
        {
            if (ModelState.IsValid)
            {
                var newPlayer = Mapper.Map<PlayerDTO>(player);
                var createdPlayerId = await _fmService.AddNewPlayerAsync(newPlayer);
                if (createdPlayerId > 0)
                {
                    return Created($"api/players/{createdPlayerId}",
                        Mapper.Map<CreatePlayerViewModel>(newPlayer));
                }
            }

            return BadRequest("Failed to add new player");
        }
    }
}