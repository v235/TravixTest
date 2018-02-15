using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FM.DAL.Models;
using FM.Services.Models;

namespace FM.Services
{
    public interface IFMService
    {
        Task<IEnumerable<PlayerDTO>> GetAllPlayersOfTheTeamAsync(int teamId);

        Task<IEnumerable<PlayerDTO>> GetAllPlayersAsync();

        Task<PlayerDTO> GetPlayerAsync(int playerId);

        Task<IEnumerable<TeamDTO>> GetTeamsAsync();

        Task<TeamDTO> GetTeamAsync(int teamId);

        Task<int> AddNewPlayerAsync(PlayerDTO newPlayer);

        Task<int> AddNewTeamAsync(TeamDTO newTeam);

        Task<bool> UpdateTeamValueAsync(TeamDTO newTeamValue);

        Task<bool> DeleteTeamAsync(int teamId);
    }
}

