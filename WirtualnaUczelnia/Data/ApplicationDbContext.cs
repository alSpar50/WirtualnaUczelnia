using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Data
{
    // ZMIANA: Dziedziczymy po IdentityDbContext (co oznacza IdentityUser domyślnie)
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Transition> Transitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // TO JEST KLUCZOWE DLA IDENTITY!

            // Konfiguracja relacji
            modelBuilder.Entity<Transition>()
                .HasOne(t => t.SourceLocation)
                .WithMany(l => l.Transitions)
                .HasForeignKey(t => t.SourceLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}