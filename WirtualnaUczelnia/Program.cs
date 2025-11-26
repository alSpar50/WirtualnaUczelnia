using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data; // Namespace Twoich danych (DbContext, Seeder)
using WirtualnaUczelnia.Models; // Namespace Twoich modeli

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Konfiguracja bazy danych - u¿ywamy connection stringa z appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// *** KLUCZOWA ZMIANA ***
// Zamiast AddDefaultIdentity, u¿ywamy pe³nej konfiguracji AddIdentity.
// To rozwi¹zuje problem "No service for type UserManager".
// Dodajemy te¿ AddDefaultUI, ¿eby dzia³a³y widoki logowania/rejestracji.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddRazorPages(); // Wymagane dla Identity UI

var app = builder.Build();

// Uruchomienie Seedowania danych przy starcie aplikacji
// (Upewnij siê, ¿e masz klasê DbSeeder w folderze Data)
await DbSeeder.Seed(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Uwierzytelnianie (Kto to jest?)
app.UseAuthorization();  // Autoryzacja (Co mo¿e robiæ?)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Mapowanie stron Razor (dla Identity)

app.Run();