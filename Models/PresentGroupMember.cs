using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DishMaster.Models
{
    public class PresentGroupMember
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public GroupMember GroupMember { get; set; }
        public TaskGroupRecord TaskGroupRecord { get; set; }
    }
}