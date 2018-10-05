using wie_doet_de_afwas.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroupRecord
    {
        public TaskGroupRecord()
        {
            this.MappingTasks = new List<Task>();
            this.MappingGroupMembers = new List<GroupMember>();
            this.PresentGroupMembers = new HashSet<GroupMember>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public TaskGroup TaskGroup { get; set; }

        [Required]
        public System.DateTime Date { get; set; }

        [Required]
        public IList<Task> MappingTasks { get; set; }

        [Required]
        public IList<GroupMember> MappingGroupMembers { get; set; }

        [Required]
        public IEnumerable<GroupMember> PresentGroupMembers { get; set; }

        public bool Finalized { get; set; }
    }
}