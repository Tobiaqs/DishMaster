using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class GroupMember
    {
        [Key]
        public string Id { get; set; }

        // Determines whether the Person's FullName should be used or the AnonymousName
        public bool IsAnonymous { get { return this.Person == null; } }

        public Person Person { get; set; }

        public string AnonymousName { get; set; }

        public double Score { get; set; }
        
        [Required]
        public Group Group { get; set; }

        public bool Administrator { get; set; }

        public bool AbsentByDefault { get; set; }
    }
}