using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Data
{
    public static class DbSeeder
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var config = serviceScope.ServiceProvider.GetService<IConfiguration>();
                var env = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

                var useDatabase = config?.GetValue<string>("UseDatabase") ?? "Sqlite";

                // Dla SQLite - tylko EnsureCreated (schemat), bez seedowania
                // Dane będą importowane ręcznie przez /DataTools
                if (useDatabase != "SqlServer")
                {
                    context.Database.EnsureCreated();
                    
                    // Sprawdź czy istnieje folder DataExport z danymi do importu
                    var dataExportPath = Path.Combine(env.ContentRootPath, "DataExport");
                    if (Directory.Exists(dataExportPath) && Directory.GetFiles(dataExportPath, "*.json").Any())
                    {
                        // Są dane do importu - nie seeduj, użytkownik zaimportuje przez /DataTools
                        // Tylko napraw ewentualne problemy z istniejącymi danymi
                        if (context.Buildings.Any())
                        {
                            var transitionsWithZeroCost = await context.Transitions.Where(t => t.Cost == 0).ToListAsync();
                            if (transitionsWithZeroCost.Any())
                            {
                                foreach (var transition in transitionsWithZeroCost)
                                {
                                    transition.Cost = 10;
                                }
                                await context.SaveChangesAsync();
                            }
                        }
                        return;
                    }
                    
                    // Brak danych do importu i pusta baza - seeduj podstawowe dane
                    if (!context.Buildings.Any())
                    {
                        await SeedBasicData(context, userManager, roleManager);
                    }
                    return;
                }

                // Dla SQL Server - standardowe działanie (bez zmian)
                // Baza już istnieje - nie rób nic specjalnego
            }
        }

        private static async Task SeedBasicData(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1. Tworzenie Roli Admina
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. Seedowanie Użytkownika Admina
            var adminEmail = "admin@wlodkowic.pl";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(newAdmin, "haslo1234PL!?");
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }

            // 3. Seedowanie Budynków
            if (!context.Buildings.Any())
            {
                var buildings = new List<Building>
                {
                    new Building { Symbol = "A", Name = "Bud. Rektorat (A)", Description = "Powierzchnia 3160 m²." },
                    new Building { Symbol = "B", Name = "Budynek B", Description = "Powierzchnia 912 m²." },
                    new Building { Symbol = "C", Name = "Budynek C", Description = "Powierzchnia 1120 m²." },
                    new Building { Symbol = "D", Name = "Biblioteka", Description = "Powierzchnia 897 m²." },
                    new Building { Symbol = "E", Name = "Wydział Pedagogiczny", Description = "Powierzchnia 1696 m²." },
                    new Building { Symbol = "F", Name = "Budynek F", Description = "Powierzchnia 1680 m²." },
                    new Building { Symbol = "G", Name = "Studium Języków Obcych", Description = "Powierzchnia 1320 m²." },
                    new Building { Symbol = "H", Name = "Centrum Sportowo-Rekreacyjne", Description = "Powierzchnia 3356 m²." }
                };
                context.Buildings.AddRange(buildings);
                await context.SaveChangesAsync();
            }
        }
    }
}