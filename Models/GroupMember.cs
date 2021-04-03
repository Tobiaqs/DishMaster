using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DishMaster.Models
{
    public class GroupMember
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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