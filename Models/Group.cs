using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wie_doet_de_afwas.Models
{
    public class Group
    {        
        [Key]
        public string Id { get; set; }
        
        [Required, Display(Name = "Group Name")]
        public string Name { get; set; }

        [Required]
        public ICollection<TaskGroup> TaskGroups { get; } = new HashSet<TaskGroup>();

        [Required]
        public ICollection<GroupMember> GroupMembers { get; } = new HashSet<GroupMember>();

        public string InvitationSecret { get; set; }

        public System.DateTime InvitationExpiration { get; set; }
    }
}