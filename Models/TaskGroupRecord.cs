using wie_doet_de_afwas.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroupRecord
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public virtual TaskGroup TaskGroup { get; set; }

        [Required]
        public System.DateTime Date { get; set; }

        [Required]
        public virtual IList<Task> MappingTasks { get; set; }

        [Required]
        public virtual IList<GroupMember> MappingGroupMembers { get; set; }

        [Required]
        public virtual ICollection<GroupMember> PresentGroupMembers { get; set; }

        public bool Finalized { get; set; }
    }
}