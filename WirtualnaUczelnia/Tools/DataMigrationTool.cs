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
        /// <summary>
        /// Eksportuje wszystkie dane z bazy do plików JSON.
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

            // Eksport Locations
            var locations = await context.Locations.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "locations.json"),
                JsonSerializer.Serialize(locations, options));

            // Eksport Transitions
            var transitions = await context.Transitions.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "transitions.json"),
                JsonSerializer.Serialize(transitions, options));

            // Eksport UserPreferences
            var userPrefs = await context.UserPreferences.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "userpreferences.json"),
                JsonSerializer.Serialize(userPrefs, options));

            // Eksport Identity Users
            var users = await context.Users.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "users.json"),
                JsonSerializer.Serialize(users, options));

            // Eksport Identity Roles
            var roles = await context.Roles.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "roles.json"),
                JsonSerializer.Serialize(roles, options));

            // Eksport Identity UserRoles
            var userRoles = await context.UserRoles.AsNoTracking().ToListAsync();
            await File.WriteAllTextAsync(
                Path.Combine(exportPath, "userroles.json"),
                JsonSerializer.Serialize(userRoles, options));
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

            if (!Directory.Exists(importPath))
            {
                throw new DirectoryNotFoundException($"Folder {importPath} nie istnieje!");
            }

            // WA¯NE: Wy³¹cz sprawdzanie kluczy obcych na czas importu
            await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF;");

            try
            {
                // Czyœcimy wszystkie tabele w odpowiedniej kolejnoœci
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Transitions;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Locations;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Buildings;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM UserPreferences;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserRoles;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserClaims;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserLogins;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserTokens;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUsers;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetRoleClaims;");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetRoles;");

                // Import Roles
                var rolesFile = Path.Combine(importPath, "roles.json");
                if (File.Exists(rolesFile))
                {
                    var rolesJson = await File.ReadAllTextAsync(rolesFile);
                    var roles = JsonSerializer.Deserialize<List<IdentityRole>>(rolesJson, options);
                    if (roles != null && roles.Any())
                    {
                        context.Roles.AddRange(roles);
                        await context.SaveChangesAsync();
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
                        context.Users.AddRange(users);
                        await context.SaveChangesAsync();
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
                        context.UserRoles.AddRange(userRoles);
                        await context.SaveChangesAsync();
                    }
                }

                // Import Buildings
                var buildingsFile = Path.Combine(importPath, "buildings.json");
                if (File.Exists(buildingsFile))
                {
                    var buildingsJson = await File.ReadAllTextAsync(buildingsFile);
                    var buildings = JsonSerializer.Deserialize<List<Building>>(buildingsJson, options);
                    if (buildings != null && buildings.Any())
                    {
                        foreach (var building in buildings)
                        {
                            building.Locations = new List<Location>();
                        }
                        context.Buildings.AddRange(buildings);
                        await context.SaveChangesAsync();
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
                        foreach (var location in locations)
                        {
                            location.Building = null;
                            location.Transitions = new List<Transition>();
                        }
                        context.Locations.AddRange(locations);
                        await context.SaveChangesAsync();
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
                        foreach (var transition in transitions)
                        {
                            transition.SourceLocation = null!;
                            transition.TargetLocation = null!;
                        }
                        context.Transitions.AddRange(transitions);
                        await context.SaveChangesAsync();
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
                        context.UserPreferences.AddRange(userPrefs);
                        await context.SaveChangesAsync();
                    }
                }
            }
            finally
            {
                // W³¹cz z powrotem sprawdzanie kluczy obcych
                await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;");
            }
        }
    }
}
