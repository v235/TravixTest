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
    [Route("api/teams/{teamName}/players")]
    public class PlayerController : Controller
    {
        private readonly IFMService _fmService;
        private readonly ILogger<PlayerController> _logger;
        public PlayerController(IFMService fmService, ILogger<PlayerController> logger)
        {
            _fmService = fmService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string teamName)
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<CreatePlayerViewModel>>
                    (await _fmService.GetAllPlayersOfTheTeam(teamName)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get players: {0}", ex);
            }
            return BadRequest("Failed to get players");
        }

        [HttpPost]
        public async Task<IActionResult> Post(string teamName, [FromBody] CreatePlayerViewModel player)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newPlayer = Mapper.Map<PlayerDTO>(player);

                    if (await _fmService.AddNewPlayerToTeam(teamName, newPlayer))
                    {
                        return Created($"api/teams/{teamName}/players/{newPlayer.Name}",
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