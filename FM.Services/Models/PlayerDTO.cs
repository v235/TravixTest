using System;
using System.Collections.Generic;
using System.Text;

namespace FM.Services.Models
{
    public class PlayerDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public int Age { get; set; }

        public int? TeamId { get; set; }

        public TeamDTO Team { get; set; }
    }
}
