using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace wie_doet_de_afwas.Models
{
    public class Person : IdentityUser
    {
        [Required, MinLength(1)]
        public string FullName { get; set; }

        [Required]
        public ICollection<GroupMember> GroupMembers { get; } = new HashSet<GroupMember>();
    }
}