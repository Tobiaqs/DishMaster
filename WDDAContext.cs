using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas
{
    public partial class WDDAContext : IdentityDbContext<Person>
    {
        public WDDAContext(DbContextOptions<WDDAContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Whenever a TaskGroupRecord is removed, do not destroy the GroupMembers it holds
            builder.Entity<TaskGroupRecord>()
                .HasMany(tgr => tgr.PresentGroupMembers)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            // Whenever a Person is removed, set it to null in any GroupMember that contains it
            builder.Entity<Person>()
                .HasMany(p => p.GroupMembers)
                .WithOne(gm => gm.Person)
                .OnDelete(DeleteBehavior.SetNull);

            // Whenever a Group is removed, remove its group members too
            builder.Entity<Group>()
                .HasMany(g => g.GroupMembers)
                .WithOne(g => g.Group)
                .OnDelete(DeleteBehavior.Cascade);

            // Whenever a Task is removed, set it to null in any TaskGroupMemberLink that contains it
            builder.Entity<Task>()
                .HasMany(typeof(TaskGroupMemberLink))
                .WithOne("Task")
                .OnDelete(DeleteBehavior.SetNull);

            // Whenever a GroupMember is removed, set it to null in any TaskGroupMemberLink that contains it
            builder.Entity<GroupMember>()
                .HasMany(typeof(TaskGroupMemberLink))
                .WithOne("GroupMember")
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<PresentGroupMember>()
                .HasOne(pgm => pgm.GroupMember)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PresentGroupMember>()
                .HasOne(pgm => pgm.TaskGroupRecord)
                .WithMany(tgr => tgr.PresentGroupMembers)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskGroup> TaskGroups { get; set; }
        public DbSet<TaskGroupRecord> TaskGroupRecords { get; set; }
        public DbSet<TaskGroupMemberLink> TaskGroupMemberLinks { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<PresentGroupMember> PresentGroupMembers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Person> Persons { get; set; }
    }
}
