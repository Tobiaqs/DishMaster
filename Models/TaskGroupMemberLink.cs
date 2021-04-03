using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DishMaster.Models
{
    public class TaskGroupMemberLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        
        public Task Task { get; set; }

        // Bounty at the time of finalization
        public int ThenBounty { get; set; }

        public GroupMember GroupMember { get; set; }

        [Required]
        public TaskGroupRecord TaskGroupRecord { get; set; }
    }
}