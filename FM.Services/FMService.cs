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

        public async Task<IEnumerable<PlayerDTO>> GetAllPlayersOfTheTeamAsync(int teamId)
        {
            var players = (await _teamRepository.GetByIdAsync(teamId)).Players.OrderBy(p => p.Name).ToList();
            return Mapper.Map<IEnumerable<PlayerDTO>>(players);
        }

        public async Task<IEnumerable<PlayerDTO>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<PlayerDTO>>(players);
        }

        public async Task<PlayerDTO> GetPlayerAsync(int playerId)
        {
            return Mapper.Map<PlayerDTO>(await _playerRepository.GetByIdAsync(playerId));
        }

        public async Task<int> AddNewPlayerAsync(PlayerDTO newPlayer)
        {
            var createdPlayer= await _playerRepository.CreateAsync(Mapper.Map<EntityPlayer>(newPlayer));
            if (createdPlayer != null)
                return createdPlayer.Id;
            return 0;
        }


        public async Task<IEnumerable<TeamDTO>> GetTeamsAsync()
        {
            return Mapper.Map<IEnumerable<TeamDTO>>(await _teamRepository.GetAllAsync());
        }

        public async Task<TeamDTO> GetTeamAsync(int teamId)
        {
            return Mapper.Map<TeamDTO>(await _teamRepository.GetByIdAsync(teamId));
        }

        public async Task<int> AddNewTeamAsync(TeamDTO newTeam)
        {
            var createdTeam = await _teamRepository.CreateAsync(Mapper.Map<EntityTeam>(newTeam));
            if (createdTeam != null)
                return createdTeam.Id;
            return 0;
        }

        public async Task<bool> UpdateTeamValueAsync(TeamDTO newTeamValue)
        {
            return await _teamRepository.UpdateAsync(Mapper.Map<EntityTeam>(newTeamValue));
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            TeamDTO teamToDelete = new TeamDTO()
            {
                Id = teamId
            };
            return await _teamRepository.DeleteAsync(Mapper.Map<EntityTeam>(teamToDelete));
        }
    }
}
