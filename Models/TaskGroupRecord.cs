using wie_doet_de_afwas.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroupRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required]
        public TaskGroup TaskGroup { get; set; }

        [Required]
        public System.DateTime Date { get; set; }

        [Required]
        public ICollection<TaskGroupMemberLink> TaskGroupMemberLinks { get; set; }

        [Required]
        public ICollection<PresentGroupMember> PresentGroupMembers { get; set; }

        public bool Finalized { get; set; }
    }
}