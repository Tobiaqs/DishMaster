using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroup
    {
        [Key]
        public string Id { get; set; }
        
        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<Task> Tasks { get; } = new List<Task>();

        [Required]
        public virtual Group Group { get; set; }
    }
}