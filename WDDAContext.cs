using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace wie_doet_de_afwas
{
    public partial class WDDAContext : IdentityDbContext<User>
    {
        public WDDAContext()
        {
        }

        public WDDAContext(DbContextOptions<WDDAContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {}
    }
}
