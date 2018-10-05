using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wie_doet_de_afwas.Models
{
    public class Group
    {
        public Group()
        {
            this.TaskGroups = new HashSet<TaskGroup>();
        }

        [Key]
        public string Id { get; set; }
        
        [Required, Display(Name = "Group Name")]
        public string Name { get; set; }

        [Required]
        public IEnumerable<TaskGroup> TaskGroups { get; set; }

        public string InvitationSecret { get; set; }

        public System.DateTime InvitationExpiration { get; set; }
    }
}