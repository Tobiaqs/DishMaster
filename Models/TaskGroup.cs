using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        
        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required]
        public ICollection<Task> Tasks { get; } = new HashSet<Task>();

        [Required]
        public ICollection<TaskGroupRecord> TaskGroupRecords { get; } = new HashSet<TaskGroupRecord>();

        [Required]
        public Group Group { get; set; }
    }
}