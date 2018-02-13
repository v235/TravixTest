﻿using System;
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
        private readonly ILogger<PlayerController> _logger;
        public PlayerController(IFMService fmService, ILogger<PlayerController> logger)
        {
            _fmService = fmService;
            _logger = logger;
        }

        [HttpGet("api/teams/{teamId}/players")]
        public async Task<IActionResult> Get(int teamId)
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<CreatePlayerViewModel>>
                    (await _fmService.GetAllPlayersOfTheTeam(teamId)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get players: {0}", ex);
            }
            return BadRequest("Failed to get players");
        }

        [HttpGet("api/teams/{teamId}/players/{playerId}")]
        public async Task<IActionResult> GetById(int playerId)
        {
            try
            {
                return Ok(Mapper.Map<CreatePlayerViewModel>
                    (await _fmService.GetPlayer(playerId)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get player: {0}", ex);
            }
            return BadRequest("Failed to get player");
        }

        [HttpPost("api/teams/addNewPlayer")]
        public async Task<IActionResult> Post([FromBody] CreatePlayerViewModel player)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newPlayer = Mapper.Map<PlayerDTO>(player);
                    var createdPlayerId = await _fmService.AddNewPlayer(newPlayer);
                    if (createdPlayerId>0)
                    {
                        return Created($"api/teams/{player.TeamId}/players/{createdPlayerId}",
                            Mapper.Map<CreatePlayerViewModel>(newPlayer));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add new player: {0}", ex);
            }
            return BadRequest("Failed to add new player");
        }
    }
}