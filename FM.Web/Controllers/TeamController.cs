using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FM.Services;
using FM.Services.Models;
using FM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FM.Web.Controllers
{
    [Route("api/teams")]
    public class TeamController : Controller
    {
        private readonly IFMService _fmService;

        public TeamController(IFMService fmService)
        {
            _fmService = fmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var teams = Mapper.Map<IEnumerable<CreateTeamViewModel>>(await _fmService.GetTeamsAsync());
            if (teams.Any())
                return Ok(teams);
            return BadRequest("Failed to get teams");
        }

        [HttpGet("{teamId}/players")]
        public async Task<IActionResult> GetPlayersAsync(int teamId)
        {
            var players = Mapper.Map<IEnumerable<CreatePlayerViewModel>>
                (await _fmService.GetAllPlayersOfTheTeamAsync(teamId));
            if (players.Any())
                return Ok(players);
            return BadRequest("Failed to get players of the team");
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetAsync(int teamId)
        {
            var team = Mapper.Map<CreateTeamViewModel>(await _fmService.GetTeamAsync(teamId));
            if (team != null)
                return Ok(team);
            return BadRequest("Failed to get team");
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateTeamViewModel team)
        {
            if (ModelState.IsValid)
            {
                var newTeam = Mapper.Map<TeamDTO>(team);
                var createdTeamId = await _fmService.AddNewTeamAsync(newTeam);
                if (createdTeamId > 0)
                    return Created($"api/teams/{createdTeamId}", Mapper.Map<CreateTeamViewModel>(newTeam));
            }

            return BadRequest("Failed to save new team");

        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateTeamViewModel team)
        {
            if (ModelState.IsValid)
            {
                var newTeamValue = Mapper.Map<TeamDTO>(team);
                if (await _fmService.UpdateTeamValueAsync(newTeamValue))
                {
                    return Ok(Mapper.Map<UpdateTeamViewModel>(newTeamValue));
                }
            }

            return BadRequest("Failed to update team values");

        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteAsync(int teamId)
        {

            if (ModelState.IsValid)
            {
                if (await _fmService.DeleteTeamAsync(teamId))
                {
                    return Ok();
                }
            }

            return BadRequest("Failed to delete the team");

        }
    }
}