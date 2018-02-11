using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FM.Web.Models
{
    public class UpdateTeamViewModel : CreateTeamViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}
