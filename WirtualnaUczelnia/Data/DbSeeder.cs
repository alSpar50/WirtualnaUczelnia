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

                // Jeśli baza nie istnieje, tworzy ją. Jeśli są migracje, aplikuje je.
                context.Database.Migrate();

                // Sprawdź, czy są już jakieś lokalizacje. Jeśli tak, nic nie rób.
                if (!context.Locations.Any())
                {
                    // 1. Tworzymy Lokalizacje
                    var locWejscie = new Location
                    {
                        Name = "Wejście Główne",
                        Description = "Stoisz przed głównym wejściem do uczelni. Widzisz duże szklane drzwi.",
                        ImageFileName = "wejscie.jpg", // Pamiętaj, żeby wgrać taki plik do wwwroot/images!
                        AudioFileName = "wejscie.mp3"
                    };

                    var locHol = new Location
                    {
                        Name = "Hol Główny",
                        Description = "Jesteś w przestronnym holu. Na wprost widzisz schody, po lewej portiernię.",
                        ImageFileName = "hol.jpg",
                        AudioFileName = "hol.mp3"
                    };

                    var locPortiernia = new Location
                    {
                        Name = "Portiernia",
                        Description = "Tutaj możesz odebrać klucze. Pani portierka uśmiecha się do Ciebie.",
                        ImageFileName = "portiernia.jpg",
                        AudioFileName = "portiernia.mp3"
                    };

                    var locSchody = new Location
                    {
                        Name = "Schody na 1. piętro",
                        Description = "Schody prowadzą do sal wykładowych i dziekanatu.",
                        ImageFileName = "schody.jpg",
                        AudioFileName = "schody.mp3"
                    };

                    // Dodajemy lokalizacje do kontekstu (żeby dostały ID)
                    context.Locations.AddRange(locWejscie, locHol, locPortiernia, locSchody);
                    context.SaveChanges();

                    // 2. Tworzymy Przejścia (Powiązania)
                    // Z Wejścia -> Prosto -> Hol
                    context.Transitions.Add(new Transition
                    {
                        SourceLocationId = locWejscie.Id,
                        TargetLocationId = locHol.Id,
                        Direction = Direction.Forward
                    });

                    // Z Holu -> W tył -> Wejście
                    context.Transitions.Add(new Transition
                    {
                        SourceLocationId = locHol.Id,
                        TargetLocationId = locWejscie.Id,
                        Direction = Direction.Back
                    });

                    // Z Holu -> W lewo -> Portiernia
                    context.Transitions.Add(new Transition
                    {
                        SourceLocationId = locHol.Id,
                        TargetLocationId = locPortiernia.Id,
                        Direction = Direction.Left
                    });

                    // Z Portierni -> W prawo -> Hol
                    context.Transitions.Add(new Transition
                    {
                        SourceLocationId = locPortiernia.Id,
                        TargetLocationId = locHol.Id,
                        Direction = Direction.Right // Wracamy do holu
                    });

                    // Z Holu -> Prosto -> Schody
                    context.Transitions.Add(new Transition
                    {
                        SourceLocationId = locHol.Id,
                        TargetLocationId = locSchody.Id,
                        Direction = Direction.Forward
                    });

                    // Z Schodów -> W tył -> Hol
                    context.Transitions.Add(new Transition
                    {
                        SourceLocationId = locSchody.Id,
                        TargetLocationId = locHol.Id,
                        Direction = Direction.Back
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}