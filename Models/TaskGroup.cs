using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroup
    {
        public TaskGroup()
        {
            this.Tasks = new HashSet<Task>();
        }

        [Key]
        public string Id { get; set; }
        
        [Required, MinLength(1)]
        [Display(Name = "Task Group Name")]
        public string Name { get; set; }

        [Required]
        public IEnumerable<Task> Tasks { get; set; }

        [Required]
        public Group Group { get; set; }
    }
}