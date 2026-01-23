using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Tools
{
    /// <summary>
    /// Narzêdzie do eksportu danych z bazy SQL Server do plików JSON
    /// oraz importu do SQLite.
    /// </summary>
    public static class DataMigrationTool
    {
        private const string ExportFolder = "DataExport";

        /// <summary>
        /// Eksportuje wszystkie dane z bazy do plików JSON.
        /// Wywo³aj rêcznie przez endpoint lub konsolê.
        /// </summary>
        public static async Task ExportDataAsync(ApplicationDbContext context, string exportPath)
        {
            Directory.CreateDirectory(exportPath);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            // Eksport Buildings
            var buildings = await context.Buildings.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "buildings.json"),
                JsonSerializer.Serialize(buildings, options));
            Console.WriteLine($"Wyeksportowano {buildings.Count} budynków");

            // Eksport Locations
            var locations = await context.Locations.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "locations.json"),
                JsonSerializer.Serialize(locations, options));
            Console.WriteLine($"Wyeksportowano {locations.Count} lokacji");

            // Eksport Transitions
            var transitions = await context.Transitions.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "transitions.json"),
                JsonSerializer.Serialize(transitions, options));
            Console.WriteLine($"Wyeksportowano {transitions.Count} przejœæ");

            // Eksport UserPreferences
            var userPrefs = await context.UserPreferences.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "userpreferences.json"),
                JsonSerializer.Serialize(userPrefs, options));
            Console.WriteLine($"Wyeksportowano {userPrefs.Count} preferencji u¿ytkowników");

            // Eksport Identity Users
            var users = await context.Users.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "users.json"),
                JsonSerializer.Serialize(users, options));
            Console.WriteLine($"Wyeksportowano {users.Count} u¿ytkowników");

            // Eksport Identity Roles
            var roles = await context.Roles.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "roles.json"),
                JsonSerializer.Serialize(roles, options));
            Console.WriteLine($"Wyeksportowano {roles.Count} ról");

            // Eksport Identity UserRoles
            var userRoles = await context.UserRoles.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "userroles.json"),
                JsonSerializer.Serialize(userRoles, options));
            Console.WriteLine($"Wyeksportowano {userRoles.Count} przypisañ ról");

            Console.WriteLine($"\nDane wyeksportowane do folderu: {exportPath}");
        }

        /// <summary>
        /// Importuje dane z plików JSON do bazy danych.
        /// </summary>
        public static async Task ImportDataAsync(ApplicationDbContext context, string importPath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // SprawdŸ czy folder istnieje
            if (!Directory.Exists(importPath))
            {
                Console.WriteLine($"Folder {importPath} nie istnieje!");
                return;
            }

            // Import Roles (najpierw role, bo u¿ytkownicy mog¹ je potrzebowaæ)
            var rolesFile = Path.Combine(importPath, "roles.json");
            if (File.Exists(rolesFile))
            {
                var rolesJson = await File.ReadAllTextAsync(rolesFile);
                var roles = JsonSerializer.Deserialize<List<IdentityRole>>(rolesJson, options);
                if (roles != null && roles.Any())
                {
                    foreach (var role in roles)
                    {
                        if (!await context.Roles.AnyAsync(r => r.Id == role.Id))
                        {
                            context.Roles.Add(role);
                        }
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaimportowano {roles.Count} ról");
                }
            }

            // Import Users
            var usersFile = Path.Combine(importPath, "users.json");
            if (File.Exists(usersFile))
            {
                var usersJson = await File.ReadAllTextAsync(usersFile);
                var users = JsonSerializer.Deserialize<List<IdentityUser>>(usersJson, options);
                if (users != null && users.Any())
                {
                    foreach (var user in users)
                    {
                        if (!await context.Users.AnyAsync(u => u.Id == user.Id))
                        {
                            context.Users.Add(user);
                        }
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaimportowano {users.Count} u¿ytkowników");
                }
            }

            // Import UserRoles
            var userRolesFile = Path.Combine(importPath, "userroles.json");
            if (File.Exists(userRolesFile))
            {
                var userRolesJson = await File.ReadAllTextAsync(userRolesFile);
                var userRoles = JsonSerializer.Deserialize<List<IdentityUserRole<string>>>(userRolesJson, options);
                if (userRoles != null && userRoles.Any())
                {
                    foreach (var ur in userRoles)
                    {
                        if (!await context.UserRoles.AnyAsync(x => x.UserId == ur.UserId && x.RoleId == ur.RoleId))
                        {
                            context.UserRoles.Add(ur);
                        }
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaimportowano {userRoles.Count} przypisañ ról");
                }
            }

            // Import Buildings (najpierw budynki, bo lokacje siê do nich odwo³uj¹)
            var buildingsFile = Path.Combine(importPath, "buildings.json");
            if (File.Exists(buildingsFile))
            {
                var buildingsJson = await File.ReadAllTextAsync(buildingsFile);
                var buildings = JsonSerializer.Deserialize<List<Building>>(buildingsJson, options);
                if (buildings != null && buildings.Any())
                {
                    // W³¹cz IDENTITY_INSERT dla SQLite nie jest potrzebne
                    // Ale musimy upewniæ siê, ¿e ID s¹ zachowane
                    context.Database.ExecuteSqlRaw("DELETE FROM Buildings");
                    
                    foreach (var building in buildings)
                    {
                        building.Locations = new List<Location>(); // Wyczyœæ nawigacjê
                        context.Buildings.Add(building);
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaimportowano {buildings.Count} budynków");
                }
            }

            // Import Locations
            var locationsFile = Path.Combine(importPath, "locations.json");
            if (File.Exists(locationsFile))
            {
                var locationsJson = await File.ReadAllTextAsync(locationsFile);
                var locations = JsonSerializer.Deserialize<List<Location>>(locationsJson, options);
                if (locations != null && locations.Any())
                {
                    context.Database.ExecuteSqlRaw("DELETE FROM Locations");
                    
                    foreach (var location in locations)
                    {
                        location.Building = null; // Wyczyœæ nawigacjê
                        location.Transitions = new List<Transition>();
                        context.Locations.Add(location);
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaimportowano {locations.Count} lokacji");
                }
            }

            // Import Transitions
            var transitionsFile = Path.Combine(importPath, "transitions.json");
            if (File.Exists(transitionsFile))
            {
                var transitionsJson = await File.ReadAllTextAsync(transitionsFile);
                var transitions = JsonSerializer.Deserialize<List<Transition>>(transitionsJson, options);
                if (transitions != null && transitions.Any())
                {
                    context.Database.ExecuteSqlRaw("DELETE FROM Transitions");
                    
                    foreach (var transition in transitions)
                    {
                        transition.SourceLocation = null!; // Wyczyœæ nawigacjê
                        transition.TargetLocation = null!;
                        context.Transitions.Add(transition);
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaimportowano {transitions.Count} przejœæ");
                }
            }

            // Import UserPreferences
            var userPrefsFile = Path.Combine(importPath, "userpreferences.json");
            if (File.Exists(userPrefsFile))
            {
                var userPrefsJson = await File.ReadAllTextAsync(userPrefsFile);
                var userPrefs = JsonSerializer.Deserialize<List<UserPreference>>(userPrefsJson, options);
                if (userPrefs != null && userPrefs.Any())
                {
                    context.Database.ExecuteSqlRaw("DELETE FROM UserPreferences");
                    
                    foreach (var pref in userPrefs)
                    {
                        context.UserPreferences.Add(pref);
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaimportowano {userPrefs.Count} preferencji u¿ytkowników");
                }
            }

            Console.WriteLine("\nImport zakoñczony!");
        }
    }
}
