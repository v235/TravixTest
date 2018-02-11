using FM.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FM.DAL.Models;
using FM.Services.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace FM.Services
{
    public class FMService : IFMService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        public FMService(ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<PlayerDTO>> GetAllPlayersOfTheTeam(string teamName)
        {
            var players = await _teamRepository.GetTeamByName(teamName);
            return Mapper.Map<IEnumerable<PlayerDTO>>(players.Players.OrderBy(p => p.Name)).ToList();
        }

        public async Task<bool> AddNewPlayerToTeam(string teamName, PlayerDTO newPlayer)
        {
            var team = await _teamRepository.GetTeamByName(teamName);
            if (team != null)
            {
                team.Players.Add(Mapper.Map<EntityPlayer>(newPlayer));
                return await _playerRepository.Create(Mapper.Map<EntityPlayer>(newPlayer));
            }

            return false;
        }


        public async Task<IEnumerable<TeamDTO>> GetTeams()
        {
            return Mapper.Map<IEnumerable<TeamDTO>>(await _teamRepository.GetAll());
        }

        public async Task<TeamDTO> GetTeam(int id)
        {
            return Mapper.Map<TeamDTO>(await _teamRepository.GetById(id));
        }

        public async Task<bool> AddNewTeam(TeamDTO newTeam)
        {
            return await _teamRepository.Create(Mapper.Map<EntityTeam>(newTeam));
        }

        public async Task<bool> UpdateTeamValue(TeamDTO newTeamValue)
        {
            return await _teamRepository.Update(Mapper.Map<EntityTeam>(newTeamValue));
        }

        public async Task<bool> DeleteTeam(string teamName)
        {
            var teamToDelete = await _teamRepository.GetTeamByName(teamName);
            if (teamToDelete != null)
            {
                return await _teamRepository.Delete(teamToDelete);
            }

            return false;
        }


    }
}
