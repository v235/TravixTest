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

        public async Task<IEnumerable<PlayerDTO>> GetAllPlayersOfTheTeam(int teamId)
        {
            var players = (await _teamRepository.GetById(teamId)).Players.OrderBy(p => p.Name).ToList();
            return Mapper.Map<IEnumerable<PlayerDTO>>(players);
        }

        public async Task<PlayerDTO> GetPlayer(int playerId)
        {
            return Mapper.Map<PlayerDTO>(await _playerRepository.GetById(playerId));
        }

        public async Task<int> AddNewPlayer(PlayerDTO newPlayer)
        {
            var createdPlayer= await _playerRepository.Create(Mapper.Map<EntityPlayer>(newPlayer));
            if (createdPlayer != null)
                return createdPlayer.Id;
            return 0;
        }


        public async Task<IEnumerable<TeamDTO>> GetTeams()
        {
            return Mapper.Map<IEnumerable<TeamDTO>>(await _teamRepository.GetAll());
        }

        public async Task<TeamDTO> GetTeam(int teamId)
        {
            return Mapper.Map<TeamDTO>(await _teamRepository.GetById(teamId));
        }

        public async Task<int> AddNewTeam(TeamDTO newTeam)
        {
            var createdTeam = await _teamRepository.Create(Mapper.Map<EntityTeam>(newTeam));
            if (createdTeam != null)
                return createdTeam.Id;
            return 0;
        }

        public async Task<bool> UpdateTeamValue(TeamDTO newTeamValue)
        {
            return await _teamRepository.Update(Mapper.Map<EntityTeam>(newTeamValue));
        }

        public async Task<bool> DeleteTeam(int teamId)
        {
            var teamToDelete = await _teamRepository.GetById(teamId);
            if (teamToDelete != null)
            {
                return await _teamRepository.Delete(teamToDelete);
            }

            return false;
        }
    }
}
