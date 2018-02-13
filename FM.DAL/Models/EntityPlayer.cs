using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM.DAL.Models
{
    public class EntityPlayer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(25)]
        public string Position { get; set; }

        public int Age { get; set; }

        [ForeignKey("Team")]
        public int? TeamId { get; set; }
        public virtual EntityTeam Team { get; set; }
    }
}
