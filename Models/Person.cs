using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using DishMaster.Annotations;

namespace DishMaster.Models
{
    public class Person : IdentityUser
    {
        [Required, MinLength(1)]
        public string FullName { get; set; }

        [Required]
        public ICollection<GroupMember> GroupMembers { get; } = new HashSet<GroupMember>();

        public System.DateTime ResetExpiration { get; set; }
    }
}