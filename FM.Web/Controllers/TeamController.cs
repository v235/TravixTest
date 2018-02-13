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
        private readonly ILogger<TeamController> _logger;

        public TeamController(IFMService fmService, ILogger<TeamController> logger)
        {
            _fmService = fmService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<CreateTeamViewModel>>(await _fmService.GetTeams()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get teams: {0}", ex);
            }
            return BadRequest("Failed to get teams");
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> Get(int teamId)
        {
            try
            {
                return Ok(Mapper.Map<CreateTeamViewModel>(await _fmService.GetTeam(teamId)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get team: {0}", ex);
            }
            return BadRequest("Failed to get team");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTeamViewModel team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTeam = Mapper.Map<TeamDTO>(team);
                    var createdTeamId = await _fmService.AddNewTeam(newTeam);
                    if(createdTeamId>0)
                    return Created($"api/teams/{createdTeamId}", Mapper.Map<CreateTeamViewModel>(newTeam));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new team: {0}", ex);
            }

            return BadRequest("Failed to save new team");

        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateTeamViewModel team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTeamValue = Mapper.Map<TeamDTO>(team);
                    if (await _fmService.UpdateTeamValue(newTeamValue))
                    {
                        return Ok(Mapper.Map<UpdateTeamViewModel>(newTeamValue));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update team values: {0}", ex);
            }

            return BadRequest("Failed to update team values");

        }
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> Delete(int teamId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _fmService.DeleteTeam(teamId))
                    {
                        return Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete the team: {0}", ex);
            }

            return BadRequest("Failed to delete the team");

        }
    }
}