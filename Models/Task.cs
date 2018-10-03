using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class Task
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Task Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Task Bounty")]
        public int Bounty { get; set; }
    }
}