using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Data
{
    // Użycie Primary Constructor (C# 12) eliminuje ostrzeżenie "Użyj konstruktora podstawowego"
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        // Te dwie linie są kluczowe - one tworzą tabele w bazie!
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Transition> Transitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji
            modelBuilder.Entity<Transition>()
                .HasOne(t => t.SourceLocation)
                .WithMany(l => l.Transitions)
                .HasForeignKey(t => t.SourceLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}