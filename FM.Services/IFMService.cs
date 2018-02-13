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
        Task<IEnumerable<PlayerDTO>> GetAllPlayersOfTheTeam(int id);

        Task<IEnumerable<TeamDTO>> GetTeams();

        Task<TeamDTO> GetTeam(int teamId);

        Task<int> AddNewPlayer(PlayerDTO newPlayer);

        Task<int> AddNewTeam(TeamDTO newTeam);

        Task<bool> UpdateTeamValue(TeamDTO newTeamValue);

        Task<bool> DeleteTeam(string teamName);
    }
}

