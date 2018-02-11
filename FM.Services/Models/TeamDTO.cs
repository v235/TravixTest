using System;
using System.Collections.Generic;
using System.Text;

namespace FM.Services.Models
{
    public class TeamDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PlayerDTO> Players { get; set; }
    }
}
