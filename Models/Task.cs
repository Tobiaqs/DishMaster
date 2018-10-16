using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class Task
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public int Bounty { get; set; }

        public bool IsNeutral { get; set; }

        [Required]
        public TaskGroup TaskGroup { get; set; }
    }
}