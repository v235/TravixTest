using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FM.Web.Models
{
    public class CreatePlayerViewModel
    {
        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5)]
        public string Position { get; set; }

        public int Age { get; set; }

        public int TeamId { get; set; }
    }
}

