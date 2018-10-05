using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class GroupMember
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public Person Person { get; set; }

        public float Score { get; set; }
        
        [Required]
        public Group Group { get; set; }

        public bool Administrator { get; set; }
    }
}