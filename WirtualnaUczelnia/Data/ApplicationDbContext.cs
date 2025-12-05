using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Transition> Transitions { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; } // <--- NOWOŚĆ

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transition>()
                .HasOne(t => t.SourceLocation)
                .WithMany(l => l.Transitions)
                .HasForeignKey(t => t.SourceLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}