using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FM.DAL.Models
{
    public class EntityTeam
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        public ICollection<EntityPlayer> Players { get; set; }
    }
}
