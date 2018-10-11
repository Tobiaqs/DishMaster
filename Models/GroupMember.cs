using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class GroupMember
    {
        [Key]
        public string Id { get; set; }

        // Determines whether the Person's FullName should be used or the AnonymousName
        public bool IsAnonymous { get; set; }

        public virtual Person Person { get; set; }

        public string AnonymousName { get; set; }

        public float Score { get; set; }
        
        [Required]
        public virtual Group Group { get; set; }

        public bool Administrator { get; set; }
    }
}