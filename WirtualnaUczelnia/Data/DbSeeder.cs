using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Data
{
    public static class DbSeeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                // Jeśli nie ma budynków, dodajemy je
                if (!context.Buildings.Any())
                {
                    var buildings = new List<Building>
                    {
                        new Building { Symbol = "A", Name = "Rektorat", Description = "Powierzchnia 3160 m². Znajduje się tu: Sala Senatu, Rektorat, Biuro Kanclerza, Dziekanat, Kwestura, Biuro Karier i laboratoria." },
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

                // Jeśli nie ma lokacji, dodajemy przykładowe powiązane z budynkiem A
                if (!context.Locations.Any())
                {
                    // Pobieramy ID Budynku A
                    var budynekA = context.Buildings.FirstOrDefault(b => b.Symbol == "A");

                    var locWejscie = new Location
                    {
                        Name = "Wejście Główne (Budynek A)",
                        Description = "Stoisz przed budynkiem A. To serce uczelni.",
                        ImageFileName = "wejscie_a.jpg",
                        AudioFileName = "wejscie_a.mp3",
                        BuildingId = budynekA?.Id
                    };

                    // ... tu możesz dodać więcej lokacji jak wcześniej ...

                    context.Locations.Add(locWejscie);
                    context.SaveChanges();
                }
            }
        }
    }
}