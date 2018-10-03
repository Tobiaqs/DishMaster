using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroup
    {
        [Key]
        public string Id { get; set; }
        
        [Display(Name = "Task Group Name")]
        public string Name { get; set; }

        public IEnumerable<Task> Tasks { get; set; }
    }
}