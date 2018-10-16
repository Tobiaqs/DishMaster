using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroupMemberLink
    {
        [Key]
        public string Id { get; set; }
        
        public Task Task { get; set; }

        // Bounty at the time of finalization
        public int ThenBounty { get; set; }

        [Required]
        public GroupMember GroupMember { get; set; }

        [Required]
        public TaskGroupRecord TaskGroupRecord { get; set; }
    }
}