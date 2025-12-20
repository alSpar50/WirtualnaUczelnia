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

                context.Database.Migrate();

                // 1. Tworzenie Roli Admina (jeśli nie istnieje)
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
                else
                {
                    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                // 3. Seedowanie Budynków
                if (!context.Buildings.Any())
                {
                    var buildings = new List<Building>
                    {
                        new Building { Symbol = "A", Name = "Bud. Rektorat (A)", Description = "Powierzchnia 3160 m². Znajduje się tu: Sala Senatu, Rektorat, Biuro Kanclerza, Dziekanat, Kwestura, Biuro Karier i laboratoria." },
                        new Building { Symbol = "B", Name = "Budynek B", Description = "Powierzchnia 912 m². Znajduje się tu: Szkoła 'Profesor', biuro Parlamentu Studentów, PCK, Redakcja gazety Per Contra." },
                        new Building { Symbol = "C", Name = "Budynek C", Description = "Powierzchnia 1120 m². Znajdują się tu: dwie aule, sale wykładowe, księgarnia akademicka." },
                        new Building { Symbol = "D", Name = "Biblioteka", Description = "Powierzchnia 897 m². Znajduje się tu: biblioteka z czytelnią, wypożyczalnia oraz stanowiska komputerowe." },
                        new Building { Symbol = "E", Name = "Wydział Pedagogiczny", Description = "Powierzchnia 1696 m². Znajduje się tu: Sekretariat Wydziału Pedagogicznego, aule, sale ćwiczeniowe i pracownia psychologiczna." },
                        new Building { Symbol = "F", Name = "Budynek F", Description = "Powierzchnia 1680 m². Mieści szkoły 'Profesor', aulę, sale wykładowe i pracownie komputerowe." },
                        new Building { Symbol = "G", Name = "Studium Języków Obcych", Description = "Powierzchnia 1320 m². Mieści: Studium Języków Obcych, laboratoria językowe, chór akademicki i archiwum." },
                        new Building { Symbol = "H", Name = "Centrum Sportowo-Rekreacyjne", Description = "Powierzchnia 3356 m². Hala sportowa, siłownia, sauny, gabinet kosmetyczny. Budynek w pełni przystosowany dla niepełnosprawnych." }
                    };
                    context.Buildings.AddRange(buildings);
                    context.SaveChanges();
                }

                // 4. Seedowanie Lokacji
                if (!context.Locations.Any())
                {
                    var budynekA = context.Buildings.FirstOrDefault(b => b.Symbol == "A");

                    if (budynekA != null)
                    {
                        // Tutaj możesz dodać przykładowe lokacje
                    }

                    context.SaveChanges();
                }

                // 5. NOWE: Napraw przejścia z Cost=0 (ustaw domyślny koszt 10)
                var transitionsWithZeroCost = await context.Transitions.Where(t => t.Cost == 0).ToListAsync();
                if (transitionsWithZeroCost.Any())
                {
                    foreach (var transition in transitionsWithZeroCost)
                    {
                        transition.Cost = 10; // Domyślny koszt
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}