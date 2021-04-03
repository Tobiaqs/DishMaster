using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DishMaster.Models
{
    public class Task
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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