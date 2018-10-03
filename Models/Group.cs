using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wie_doet_de_afwas.Models
{
    public class Group
    {
        [Key]
        public string Id { get; set; }
        
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        public IEnumerable<TaskGroup> TaskGroups { get; set; }
    }
}