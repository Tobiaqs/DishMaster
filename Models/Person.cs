using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace wie_doet_de_afwas.Models
{
    public class Person : IdentityUser
    {
        public string FullName { get; set; }
    }
}