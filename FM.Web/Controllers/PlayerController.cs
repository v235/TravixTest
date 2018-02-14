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
    public class PlayerController : Controller
    {
        private readonly IFMService _fmService;
        public PlayerController(IFMService fmService)
        {
            _fmService = fmService;
        }

        [HttpGet("api/teams/{teamId}/players")]
        public async Task<IActionResult> GetAsync(int teamId)
        {
            var players = Mapper.Map<IEnumerable<CreatePlayerViewModel>>
                (await _fmService.GetAllPlayersOfTheTeamAsync(teamId));
            if (players.Any())
                return Ok(players);
            return BadRequest("Failed to get players");
        }

        [HttpGet("api/teams/{teamId}/players/{playerId}")]
        public async Task<IActionResult> GetByIdAsync(int playerId)
        {
            var player = Mapper.Map<CreatePlayerViewModel>
                (await _fmService.GetPlayerAsync(playerId));
            if (player != null)
                return Ok(player);
            return BadRequest("Failed to get player"); 
        }

        [HttpPost("api/teams/addNewPlayer")]
        public async Task<IActionResult> PostAsync([FromBody] CreatePlayerViewModel player)
        {
            if (ModelState.IsValid)
            {
                var newPlayer = Mapper.Map<PlayerDTO>(player);
                var createdPlayerId = await _fmService.AddNewPlayerAsync(newPlayer);
                if (createdPlayerId > 0)
                {
                    return Created($"api/teams/{player.TeamId}/players/{createdPlayerId}",
                        Mapper.Map<CreatePlayerViewModel>(newPlayer));
                }
            }

            return BadRequest("Failed to add new player");
        }
    }
}