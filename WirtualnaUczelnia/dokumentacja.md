Szkoła Wyższa im. Pawła Włodkowica
w Płocku

Kierunek Informatyka

Programowanie aplikacji internetowych

Multimedialny przewodnik po uczelni

Patryk Ambroziak, 57890

Paulina Stankiewicz, 57899

Albert Wróbel, 57876

Płock, 2025

Multimedialny przewodnik po uczelni

1. Geneza i cel pracy
2. Analiza zadania

2.1. Charakterystyka i ogólny opis aplikacji
2.2. Określenie wymagań ogólnych
2.3. Charakterystyka grupy docelowej
2.4.   Rozwiązania konkurencyjne
2.5.   Użytkownicy i ich role w systemie

2.5.1. Gość (użytkownik niezalogowany)
2.5.2. Użytkownik zalogowany
2.5.3. Administrator

2.6.   Wymagania funkcjonalne i niefunkcjonalne
2.7.   Pozostałe wymagania
2.8. Diagramy przypadków użycia
2.8.1. Użytkownik aplikacji
2.8.2. Administrator

2.9.   Prototypy

3. Realizacja zadania projektowego

3.1.   Wykorzystane technologie i narzędzia
3.2.   Klasy modelu + walidacja danych
3.3.   Migracja danych + dane początkowe + powstanie bazy
3.4. Wirtualny spacer – moduł eksploracji
3.4.1. Funkcja wyświetlania lokalizacji
3.4.2. Funkcja nawigacji strzałkami
3.5. System nawigacji – wyznaczanie tras

3.5.1. Algorytm Dijkstry
3.5.2. Obsługa preferencji dostępności

3.6. Wyszukiwarka miejsc

3.6.1. Funkcja wyszukiwania

3.7. Panel administracyjny

3.7.1. Zarządzanie budynkami
3.7.2. Zarządzanie lokalizacjami
3.7.3. Zarządzanie przejściami

3.8. Dostępność (WCAG)

3.8.1. Wsparcie dla czytników ekranu

4. Podsumowanie i wnioski
5.  Netografia i bibliografia

5.1. Netografia
5.2. Bibliografia

6. Spis tabel, spis rysunków, spis kodu

6.1. Rysunki
6.2. Tabele
6.3. Fragmenty kodu

1
3
3
3
3
3
4
5
5
5
6
6
7
7
8
9
9
10
10
12
15
17
17
18
19
19
21
22
22
22
22
23
23
23
23
24
25
25
25
26
26
26
26

2

1. Geneza i cel pracy

Celem  niniejszej  pracy  jest  zaprojektowanie  i  implementacja  responsywnej  aplikacji
umożliwiającej  wirtualne  poruszanie  się  po  Uczelni  w  Płocku.  Aplikacja  prezentuje
rzeczywiste  zdjęcia  poszczególnych  miejsc,  uzupełnione  o  opis  oraz  informację  głosową,
pozwalając  użytkownikowi  wybrać  kierunek  dalszego  przemieszczania  się  poprzez
interakcję  z  aktywnymi  obszarami  zdjęć.  Rozwiązanie  ma  na  celu  ułatwienie  orientacji
przestrzennej  na  terenie  uczelni,  w  szczególności  dla  nowych  studentów  oraz  osób
odwiedzających  obiekt  po  raz  pierwszy,  poprzez  odwzorowanie  procesu  rzeczywistego
spaceru w formie intuicyjnej aplikacji cyfrowej.

2. Analiza zadania

2.1. Charakterystyka i ogólny opis aplikacji

Aplikacja stanowi interaktywne narzędzie umożliwiające wirtualny spacer po terenie Uczelni
w  Płocku.  Jej  działanie  opiera  się  na  sekwencyjnym  prezentowaniu  zdjęć  rzeczywistych
lokalizacji, uzupełnionych o opisy tekstowe oraz opcjonalne informacje głosowe. Użytkownik,
wybierając  budynek  początkowy,  może  przemieszczać  się  pomiędzy  kolejnymi  punktami
poprzez wskazanie kierunku ruchu za pomocą aktywnych obszarów na zdjęciach. Aplikacja
została zaprojektowana jako responsywna, co umożliwia jej poprawne działanie na różnych
urządzeniach
charakter
informacyjno-nawigacyjny  i  wspiera  orientację  przestrzenną  użytkowników  w  sposób
intuicyjny i zbliżony do rzeczywistego poruszania się po obiekcie.

rozdzielczościach

Rozwiązanie

ekranu.

ma

i

2.2. Określenie wymagań ogólnych

Aplikacja  powinna  umożliwiać  użytkownikom łatwą i intuicyjną nawigację po terenie uczelni
oraz  szybkie  sprawdzenie  lokalizacji  wybranych  sal,  pomieszczeń  i  budynków. System ma
wspierać orientację przestrzenną poprzez prezentację rzeczywistych widoków miejsc wraz z
opisem  i  opcjonalną  informacją  głosową,  odwzorowując  proces  rzeczywistego  poruszania
się po obiekcie. Aplikacja powinna działać w sposób responsywny na różnych urządzeniach,
reagować na interakcję użytkownika oraz zapewniać czytelny i prosty interfejs. Rozwiązanie
ma  charakter
i  powinno  być  dostępne  bez  konieczności  posiadania
specjalistycznej wiedzy technicznej.

informacyjny

2.3. Charakterystyka grupy docelowej

Grupą  docelową  aplikacji  są  przede  wszystkim  studenci  uczelni,  w  szczególności  osoby
nowo  przyjęte,  które  nie  znają  jeszcze  struktury  budynków  oraz  rozmieszczenia  sal  oraz
pomieszczeń.  Aplikacja  skierowana  jest  również  do  osób  odwiedzających  uczelnię  po  raz
pierwszy,  takich  jak  kandydaci  na  studia  czy  goście.  Istotną  grupę użytkowników stanowią
także  osoby  z  niepełnosprawnościami,  dla  których  tradycyjne  formy  nawigacji  mogą  być
niewystarczające  lub  utrudnione.  Aplikacja  ma  wspierać  te  grupy  poprzez  czytelne
przedstawienie  tras  i  lokalizacji  w  formie  wirtualnego  spaceru,  zwiększając  dostępność
informacji oraz komfort poruszania się po terenie uczelni.

3

Rys. 1. Persony użytkowników

2.4.   Rozwiązania konkurencyjne

Matterport
Platforma  umożliwiająca  tworzenie  zaawansowanych  spacerów  3D  opartych  na  skanach

4

jednak  wymaga
przestrzennych.  Rozwiązanie  oferuje  wysoką
specjalistycznego  sprzętu  do  skanowania,  jest  kosztowne  oraz  zamknięte technologicznie.
Brakuje  możliwości pełnej personalizacji logiki nawigacji oraz dostosowania tras do potrzeb
osób z niepełnosprawnościami w sposób specyficzny dla struktury uczelni.

jakość  wizualną,

iGUIDE
System  koncentruje  się  na prezentacji przestrzeni w formie wirtualnych wycieczek, głównie
dla  rynku  nieruchomości. Oferuje ograniczone możliwości interakcji oraz brak elastycznego
modelu  przejść  pomiędzy  lokacjami.  Rozwiązanie  nie  wspiera  zaawansowanej  logiki
nawigacyjnej  ani
tras
integracji  z
dostosowanych do użytkownika.

funkcjami  wyszukiwania  konkretnych  sal  czy

Metareal
Narzędzie ukierunkowane na tworzenie realistycznych modeli i środowisk 3D, przeznaczone
głównie  do gier i aplikacji komercyjnych. Wymaga dużych nakładów pracy projektowej oraz
wiedzy  technicznej.  Nie  stanowi  gotowego  systemu  nawigacyjnego  i  nie  odpowiada
bezpośrednio na potrzeby aplikacji informacyjnej dla uczelni.

Istniejące rozwiązania komercyjne nie spełniają w pełni wymagań aplikacji przeznaczonej do
nawigacji  po  uczelni,  w  szczególności  pod  kątem  dostępności,  elastyczności  oraz  kontroli
nad  strukturą  danych. Tworzenie systemu od podstaw umożliwia pełne dostosowanie logiki
poruszania  się  do  rzeczywistego  układu  budynków,  implementację  prostego  i  intuicyjnego
modelu  decyzyjnego  (kierunki  ruchu),  integrację  informacji  głosowych  oraz  rozwój  funkcji
dedykowanych  osobom  z  niepełnosprawnościami.  Dodatkowo  autorskie  rozwiązanie
eliminuje  koszty  licencyjne,  zależność  od  zewnętrznych  platform  oraz  pozwala  na  dalszą
rozbudowę systemu zgodnie z potrzebami uczelni.

2.5.   Użytkownicy i ich role w systemie

2.5.1. Gość (użytkownik niezalogowany)

Użytkownik,  który  korzysta  z  aplikacji  bez  logowania.  Ma  dostęp  do podstawowych funkcji
eksploracyjnych.

Wymagania:

●  Przeglądanie wirtualnej mapy uczelni.
●  Poruszanie się pomiędzy lokacjami za pomocą przejść (strzałek).
●  Wyszukiwanie lokacji (np. dziekanat, sala, budynek).
●  Wyznaczanie trasy do wybranej lokacji.

2.5.2. Użytkownik zalogowany

Użytkownik posiadający konto, mogący personalizować sposób poruszania się po uczelni.

Wymagania:

●  Wszystkie funkcje dostępne dla Gościa.
●  Logowanie i wylogowywanie z systemu.

5

●  Możliwość zapisu preferencji użytkownika:

●  status: osoba pełnosprawna / osoba z niepełnosprawnością.

●  Automatyczne  dostosowanie  tras  do  zapisanych  preferencji  (np.  wykluczenie

schodów dla osób poruszających się na wózku).

●  Edycja własnych ustawień.

2.5.3. Administrator

Użytkownik zarządzający całą strukturą aplikacji i jej danymi.

Wymagania:

●  Zarządzanie budynkami:
●  dodawanie,
●  edycja,
●  usuwanie.

●  Zarządzanie lokacjami (np. sale, dziekanaty, windy).
●  Zarządzanie przejściami między lokacjami (w tym dostępność dla osób

z niepełnosprawnościami).
●  Zarządzanie użytkownikami:
●  przeglądanie,
●  edycja,
●  usuwanie kont.

●  Pełny dostęp do danych aplikacji.

2.6.   Wymagania funkcjonalne i niefunkcjonalne

Wymagania funkcjonalne
Aplikacja  powinna  umożliwiać  użytkownikowi  przeglądanie  wirtualnej  przestrzeni  uczelni
poprzez  prezentację  zdjęć  rzeczywistych  lokalizacji.  System  musi  pozwalać  na  wybór
budynku  początkowego  oraz  przemieszczanie  się  pomiędzy  kolejnymi  miejscami  poprzez
wybór  kierunku  ruchu  (np.  prosto,  w  lewo,  w  prawo)  za  pomocą  aktywnych  obszarów  na
tekstowe  oraz  informacje  głosowe
zdjęciach.  Aplikacja  powinna  udostępniać  opisy
dotyczące  aktualnie  wyświetlanej
lokalizacji.  Użytkownik  powinien  mieć  możliwość
wyszukania  konkretnej  sali  lub  pomieszczenia  oraz  uzyskania  informacji,  gdzie  się  ono
znajduje.  System  powinien  obsługiwać  różne  role  użytkowników  (gość,  użytkownik
zalogowany,  administrator)  oraz  umożliwiać administratorowi zarządzanie strukturą uczelni,
lokacjami i przejściami między nimi.

Wymagania niefunkcjonalne
Aplikacja  powinna  być  responsywna  i  poprawnie  działać  na  różnych  urządzeniach  oraz
rozdzielczościach  ekranu.  Interfejs  użytkownika  musi  być  intuicyjny,  czytelny  i  prosty  w
obsłudze, tak aby nie wymagał wcześniejszego przeszkolenia. System powinien zapewniać
odpowiedni  czas  reakcji  na  działania  użytkownika  oraz  stabilność  działania.  Aplikacja
powinna  wspierać  dostępność,  w  szczególności  poprzez  możliwość odtwarzania informacji
głosowych  oraz  logiczną  strukturę  nawigacji.  Rozwiązanie  powinno  być  skalowalne  i
umożliwiać  dalszą  rozbudowę  funkcjonalności  bez  konieczności  przebudowy  całego
systemu.

6

2.7.   Pozostałe wymagania

Aplikacja  powinna  spełniać  obowiązujące  wymagania  prawne,  w  szczególności  dotyczące
ochrony  danych  osobowych,  jeśli  przetwarzane  są  dane  użytkowników  (np.  w  przypadku
kont zalogowanych). Wszelkie informacje prezentowane w aplikacji powinny być zgodne ze
stanem faktycznym oraz nie naruszać praw autorskich, w tym praw do wykorzystanych zdjęć
i  materiałów  dźwiękowych.  Interfejs  użytkownika  powinien  być  prosty,  czytelny  i  spójny
wizualnie, umożliwiając łatwą obsługę bez konieczności posiadania specjalistycznej wiedzy.
Aplikacja  powinna  uwzględniać  potrzeby  osób  z  niepełnosprawnościami,  m.in.  poprzez
czytelną  strukturę  nawigacji,  możliwość  korzystania  z  informacji  głosowych  oraz  unikanie
rozwiązań  utrudniających  obsługę  osobom z ograniczeniami ruchowymi lub percepcyjnymi.
System  powinien  być  zaprojektowany  w  sposób  zapewniający  komfortowe  i  bezpieczne
użytkowanie w różnych warunkach oraz na różnych urządzeniach.

2.8. Diagramy przypadków użycia

Diagramy  przypadków  użycia  przygotowano  w  programie  draw.io  na  podstawie  ról
użytkowników.

7

2.8.1. Użytkownik aplikacji

Rys. 2. Diagram przypadków użycia (użytkownik)

8

2.8.2. Administrator

Rys. 3. Diagram przypadków użycia (administrator)

2.9.   Prototypy

Prototypy  interfejsu  użytkownika  zostały  przygotowane  z  uwzględnieniem  responsywności
oraz dostępności. Poniżej przedstawiono kluczowe ekrany aplikacji:

Strona główna – Mapa Kampusu

Strona  główna  prezentuje  listę  budynków  uczelni  w  formie  kafelków  z  obrazami.  Każdy
kafelek zawiera:

zdjęcie budynku (lub domyślne tło),
symbol budynku (np. „Budynek A"),

-
-
-  nazwę budynku,
-
skrócony opis,
-  przycisk „Wejdź do środka" inicjujący wirtualny spacer.

9

U  góry  strony  znajduje  się  wyszukiwarka  umożliwiająca  szybkie  odnalezienie  konkretnego
pomieszczenia lub usługi.

Widok wirtualnego spaceru (Explore)

Główny  widok  aplikacji  prezentuje  zdjęcie  aktualnej  lokalizacji  z  nałożonymi  strzałkami
nawigacyjnymi. Interfejs zawiera:

interaktywne strzałki kierunkowe (prosto, w lewo, w prawo, w tył, w górę, w dół),

-  duże zdjęcie lokalizacji z tekstem alternatywnym dla czytników ekranu,
-
-  nazwę i opis miejsca,
-  opcjonalny odtwarzacz audio z lektorem,
-

rozwijaną listę dostępnych przejść z oznaczeniem dostępności dla wózków.

Formularz nawigacji (Znajdź drogę)

Dedykowany formularz pozwala użytkownikowi wybrać:

lokalizację startową,
lokalizację docelową,

-
-
-  opcję

„Tryb  winda"  dla  osób  z  niepełnosprawnością  (dla  niezalogowanych

użytkowników).

Zalogowani użytkownicy mają możliwość zapisania preferencji dostępności w profilu.

Wynik nawigacji (Result)

Prezentacja znalezionej trasy w formie numerowanej listy kroków. Każdy krok zawiera:

ikonę kierunku,
instrukcję tekstową,

-
-
-  przycisk „Podgląd" z tooltipem pokazującym miniaturę zdjęcia lokalizacji,
-  oznaczenie celu podróży na ostatnim kroku.

3. Realizacja zadania projektowego

3.1.   Wykorzystane technologie i narzędzia

i  zaimplementowana  z  wykorzystaniem  platformy
Aplikacja  została  zaprojektowana
ASP.NET  Core  MVC,  która  umożliwia  tworzenie  nowoczesnych  aplikacji  webowych  w
oparciu  o  architekturę  Model–View–Controller.  Warstwa  danych  oparta  jest  na  systemie
zarządzania  bazą  danych  SQL  Server,  zapewniającym  stabilne i wydajne przechowywanie
informacji  o  budynkach,  lokalizacjach,  przejściach  oraz  użytkownikach.  Do  komunikacji
pomiędzy  aplikacją  a  bazą  danych  wykorzystano  Entity  Framework,  który  umożliwia

10

mapowanie  obiektowo-relacyjne  oraz  upraszcza  operacje  na  danych.  Warstwa  interfejsu
użytkownika została wykonana przy użyciu frameworka Bootstrap, co pozwala na tworzenie
responsywnego  i  spójnego  wizualnie  interfejsu,  dostosowanego  do  różnych  urządzeń  i
rozdzielczości  ekranu.  Zastosowany zestaw technologii zapewnia skalowalność, czytelność
kodu oraz możliwość dalszego rozwoju aplikacji.

Kategoria

Technologia

Wersja

Opis

Framework

ASP.NET Core

8.0

Architektura

MVC

Baza danych

SQL Server

-

-

ORM

Uwierzytelnianie

Entity Framework
Core

ASP.NET Core
Identity

8.0.7

8.0.7

Frontend

Bootstrap

5.x

Platforma do
tworzenia
nowoczesnych
aplikacji webowych

Model–View–Control
ler

Relacyjny system
zarządzania bazą
danych

Mapowanie
obiektowo-relacyjne

Zarządzanie
użytkownikami i
rolami

Framework CSS dla
responsywnych
interfejsów

Ikony

Język

Bootstrap Icons

-

Biblioteka ikon

C#

12.0

Język
programowania

Tab. 1. Wykorzystane technologie i narzędzia

Dodatkowe pakiety NuGet:

-  Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore – obsługa

błędów EF Core

-  Microsoft.AspNetCore.Identity.UI – gotowe widoki Razor Pages dla Identity
-  Microsoft.EntityFrameworkCore.Design – narzędzia projektowe EF Core
-  Microsoft.EntityFrameworkCore.SqlServer – provider SQL Server
-  Microsoft.EntityFrameworkCore.Tools – narzędzia migracji
-  Microsoft.VisualStudio.Web.CodeGeneration.Design – scaffolding

11

3.2.   Klasy modelu + walidacja danych

Model danych aplikacji składa się z czterech głównych encji:

1. Building (Budynek)

C#

public class Building

{

    [Key]

    public int Id { get; set; }

    [Required]

    [Display(Name = "Symbol budynku")]

    public string Symbol { get; set; } // np. "A", "B", "H"

    [Display(Name = "Pełna nazwa")]

    public string Name { get; set; }

    [Display(Name = "Opis")]

    public string Description { get; set; }

    [Display(Name = "Zdjęcie budynku")]

    public string? ImageFileName { get; set; }

    [Display(Name = "Tekst alternatywny (dla czytników)")]

    [StringLength(500)]

    public string? ImageAltText { get; set; }

    [Display(Name = "Ukryty")]

    public bool IsHidden { get; set; } = false;

    public virtual ICollection<Location> Locations { get; set; }

}

Kod 1. Model Building (Budynek)

2. Location (Lokalizacja)

C#

public enum LocationType

{

    [Display(Name = "Pomieszczenie")] Room,

    [Display(Name = "Korytarz")] Corridor,

    [Display(Name = "Hol")] Hall,

    [Display(Name = "Schody")] Stairs,

    [Display(Name = "Winda")] Elevator,

12

    [Display(Name = "Wejście")] Entrance,

    [Display(Name = "Inne")] Other

}

public class Location

{

    [Key]

    public int Id { get; set; }

    [Required]

    [Display(Name = "Nazwa miejsca")]

    public string Name { get; set; }

    [Display(Name = "Opis")]

    public string Description { get; set; }

    [Required]

    [Display(Name = "Plik zdjęcia")]

    public string ImageFileName { get; set; }

    [Display(Name = "Tekst alternatywny")]

    [StringLength(500)]

    public string? ImageAltText { get; set; }

    [Display(Name = "Plik audio")]

    public string? AudioFileName { get; set; }

    [Display(Name = "Ukryta")]

    public bool IsHidden { get; set; } = false;

    [Display(Name = "Typ lokalizacji")]

    public LocationType Type { get; set; } = LocationType.Room;

    [Display(Name = "Piętro")]

    public int Floor { get; set; } = 0;

    public virtual ICollection<Transition> Transitions { get; set; }

    [Display(Name = "Budynek")]

    public int? BuildingId { get; set; }

    public virtual Building? Building { get; set; }

}

Kod 2. Model Location (Lokalizacja)

3. Transition (Przejście)

C#

public enum Direction

13

{

    [Display(Name = "Prosto")] Forward,

    [Display(Name = "W lewo")] Left,

    [Display(Name = "W prawo")] Right,

    [Display(Name = "W tył")] Back,

    [Display(Name = "W górę")] Up,

    [Display(Name = "W dół")] Down

}

public class Transition

{

    [Key]

    public int Id { get; set; }

    [Required]

    public Direction Direction { get; set; }

    [Range(0, 100)]

    public int PositionX { get; set; } = 50;

    [Range(0, 100)]

    public int PositionY { get; set; } = 80;

    [Display(Name = "Dostępne dla wózków")]

    public bool IsWheelchairAccessible { get; set; } = true;

    [Display(Name = "Ukryte")]

    public bool IsHidden { get; set; } = false;

    [Display(Name = "Koszt przejścia")]

    [Range(1, 1000)]

    public int Cost { get; set; } = 10;

    [ForeignKey("SourceLocation")]

    public int SourceLocationId { get; set; }

    public virtual Location SourceLocation { get; set; }

    [ForeignKey("TargetLocation")]

    public int TargetLocationId { get; set; }

    public virtual Location TargetLocation { get; set; }

}

Kod 3. Model Transition (Przejście)

4. UserPreference (Preferencje użytkownika)

14

C#

public class UserPreference

{

    [Key]

    public int Id { get; set; }

    public string UserId { get; set; }

    [Display(Name = "Osoba z niepełnosprawnością")]

    public bool IsDisabled { get; set; }

}

Kod 4. Model UserPreference (Preferencje użytkownika)

Walidacja danych:

-  Atrybuty [Required] wymuszają obowiązkowe pola
-  [StringLength(500)] ogranicza długość tekstów alternatywnych
-  [Range(0, 100)] waliduje pozycje strzałek (procenty)
-  [Range(1, 1000)] ogranicza wartości kosztów przejść
-  Walidacja plików w kontrolerze (rozmiar max 10 MB, dozwolone rozszerzenia)

3.3.   Migracja danych + dane początkowe + powstanie bazy

Kontekst bazy danych (ApplicationDbContext):

C#

public class ApplicationDbContext : IdentityDbContext

{

    public DbSet<Building> Buildings { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<Transition> Transitions { get; set; }

    public DbSet<UserPreference> UserPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transition>()

            .HasOne(t => t.SourceLocation)

            .WithMany(l => l.Transitions)

            .HasForeignKey(t => t.SourceLocationId)

            .OnDelete(DeleteBehavior.Restrict);

15

    }

}

Kod 5. Kontekst bazy danych ApplicationDbContext

Historia migracji:

Data

Nazwa migracji

Opis zmian

2025-11-26

InitialCreate

Utworzenie tabel: Buildings,
Locations, Transitions,
Identity

2025-12-05

AddUserPreferencesAndAc
cess

Dodanie tabeli
UserPreferences, pola
IsWheelchairAccessible

2025-12-05

AddBuildingImage

2025-12-20

AddImageAltText

2025-12-20

NewTables

2025-12-20

AddIsHiddenFields

Dodanie ImageFileName do
budynków

Dodanie pól ImageAltText
dla dostępności

Dodanie pól Type i Floor do
lokalizacji

Dodanie pól IsHidden do
wszystkich encji

2025-12-20

AddCostAndLocationType

Dodanie Cost do przejść

Tab. 2. Historia migracji bazy danych

Seedowanie danych początkowych (DbSeeder):

C#

public static async Task Seed(IApplicationBuilder applicationBuilder)

{

    // 1. Tworzenie roli Admin

    if (!await roleManager.RoleExistsAsync("Admin"))

    {

        await roleManager.CreateAsync(new IdentityRole("Admin"));

    }

    // 2. Tworzenie konta administratora

16

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

    // 3. Seedowanie budynków uczelni

    if (!context.Buildings.Any())

    {

        var buildings = new List<Building>

        {

            new Building { Symbol = "A", Name = "Bud. Rektorat (A)",

                Description = "Powierzchnia 3160 m². Sala Senatu, Rektorat..." },

            new Building { Symbol = "B", Name = "Budynek B", ... },

            // ... pozostałe budynki H, C, D, E, F, G

        };

        context.Buildings.AddRange(buildings);

    }

}

Kod 6. Seedowanie danych początkowych

3.4. Wirtualny spacer – moduł eksploracji

3.4.1. Funkcja wyświetlania lokalizacji

Kontroler ExploreController zarządza widokiem wirtualnego spaceru:

C#

public async Task<IActionResult> Index(int? id)

{

    Location location;

17

    if (id == null)

    {

        // Start od pierwszej widocznej lokalizacji

        location = await _context.Locations

            .Include(l => l.Transitions)

                .ThenInclude(t => t.TargetLocation)

            .Where(l => !l.IsHidden)

            .Where(l => l.Building == null || !l.Building.IsHidden)

            .FirstOrDefaultAsync();

    }

    else

    {

        // Pobierz konkretną lokalizację z filtrami widoczności

        location = await _context.Locations

            .Include(l => l.Building)

            .Include(l => l.Transitions)

                .ThenInclude(t => t.TargetLocation)

            .FirstOrDefaultAsync(m => m.Id == id);

    }

    // Filtrowanie ukrytych przejść

    location.Transitions = location.Transitions

        .Where(t => !t.IsHidden)

        .Where(t => t.TargetLocation != null && !t.TargetLocation.IsHidden)

        .ToList();

    return View(location);

}

Kod 7. Kontroler ExploreController – wyświetlanie lokalizacji

Widok Explore/Index.cshtml prezentuje:

-  Zdjęcie lokalizacji z tekstem alternatywnym
-  Strzałki nawigacyjne pozycjonowane procentowo (CSS absolute)
-
-  Odtwarzacz audio (opcjonalnie)
-  Rozwijaną listę przejść tekstowych

Informacje o dostępności dla wózków

3.4.2. Funkcja nawigacji strzałkami

Strzałki nawigacyjne są renderowane dynamicznie na podstawie przejść:

18

cshtml

@foreach (var transition in Model.Transitions)

{

    <a asp-action="Index" asp-route-id="@transition.TargetLocationId"

       class="nav-arrow"

       style="left: @transition.PositionX%; top: @transition.PositionY%;"

       aria-label="Strzałka @directionName. Prowadzi do: @targetName">

        <span aria-hidden="true">@arrowIcon</span>

    </a>

}

Strzałki reagują na hover (powiększenie) i fokus klawiatury (outline).

3.5. System nawigacji – wyznaczanie tras

3.5.1. Algorytm Dijkstry

Serwis PathFinderService implementuje algorytm najkrótszej ścieżki:

C#

public async Task<List<NavigationStep>> FindPathAsync(

    int startId, int endId, bool requireWheelchairAccess)

{

    // 1. Pobierz przejścia z filtrami dostępności

    var query = _context.Transitions

        .Where(t => !t.IsHidden)

        .Where(t => !t.SourceLocation.IsHidden);

    if (requireWheelchairAccess)

    {

        query = query.Where(t => t.IsWheelchairAccessible);

    }

    // 2. Budowanie grafu

    var graph = new Dictionary<int, List<(int targetId, Transition, int cost)>>();

    foreach (var t in allTransitions)

    {

        int effectiveCost = t.Cost;

19

        // Dla osób pełnosprawnych - kara dla windy (schody preferowane)

        if (!requireWheelchairAccess &&

            t.TargetLocation.Type == LocationType.Elevator)

        {

            effectiveCost += ELEVATOR_PENALTY_FOR_ABLED;

        }

        graph[t.SourceLocationId].Add((t.TargetLocationId, t, effectiveCost));

    }

    // 3. Algorytm Dijkstry z PriorityQueue

    var distances = new Dictionary<int, int>();

    var cameFrom = new Dictionary<int, Transition>();

    var priorityQueue = new PriorityQueue<int, int>();

    distances[startId] = 0;

    priorityQueue.Enqueue(startId, 0);

    while (priorityQueue.Count > 0)

    {

        var currentId = priorityQueue.Dequeue();

        if (currentId == endId) break;

        foreach (var (targetId, transition, cost) in graph[currentId])

        {

            int newDist = distances[currentId] + cost;

            if (!distances.ContainsKey(targetId) || newDist < distances[targetId])

            {

                distances[targetId] = newDist;

                cameFrom[targetId] = transition;

                priorityQueue.Enqueue(targetId, newDist);

            }

        }

    }

    // 4. Rekonstrukcja ścieżki

    var path = new List<NavigationStep>();

    var curr = endId;

    while (curr != startId && cameFrom.ContainsKey(curr))

    {

        path.Add(new NavigationStep { ... });

        curr = cameFrom[curr].SourceLocationId;

20

    }

    path.Reverse();

    return path;

}

Kod 8. Algorytm Dijkstry w PathFinderService

3.5.2. Obsługa preferencji dostępności

System obsługuje dwa tryby dostępności:

Dla zalogowanych użytkowników:

-  Preferencje zapisane w tabeli UserPreferences
-  Edycja w /Identity/Account/Manage/Accessibility

Dla niezalogowanych użytkowników:

-  Checkbox „Tryb winda" w formularzu nawigacji
-  Wartość przechowywana w sesji HTTP

C#

if (userId != null)

{

    var pref = await _context.UserPreferences

        .FirstOrDefaultAsync(p => p.UserId == userId);

    requireWheelchairAccess = pref?.IsDisabled ?? false;

}

else

{

    requireWheelchairAccess = wheelchairMode;

    HttpContext.Session.SetString(WheelchairModeSessionKey,

        wheelchairMode ? "true" : "false");

}

Kod 9. Obsługa preferencji dostępności

21

3.6. Wyszukiwarka miejsc

3.6.1. Funkcja wyszukiwania

Kontroler HomeController obsługuje wyszukiwanie lokalizacji:

C#

public async Task<IActionResult> Search(string query)

{

    if (string.IsNullOrWhiteSpace(query))

        return RedirectToAction("Index");

    var results = await _context.Locations

        .Include(l => l.Building)

        .Where(l => !l.IsHidden)

        .Where(l => l.Building == null || !l.Building.IsHidden)

        .Where(l => l.Name.Contains(query) ||

                    (l.Description != null && l.Description.Contains(query)))

        .ToListAsync();

    ViewBag.Query = query;

    return View(results);

}

Kod 10. Funkcja wyszukiwania lokalizacji

Wyszukiwarka przeszukuje nazwy i opisy lokalizacji, uwzględniając filtry widoczności.

3.7. Panel administracyjny

3.7.1. Zarządzanie budynkami

Kontroler BuildingsController (dostępny tylko dla roli Admin) zapewnia operacje
CRUD:

Index – lista wszystkich budynków (również ukrytych)

-
-  Create – dodawanie z uploadem zdjęcia
-  Edit – edycja z możliwością zmiany zdjęcia
-  ToggleVisibility – przełączanie widoczności
-  Delete – usuwanie z walidacją powiązań

C#

[Authorize(Roles = "Admin")]

22

public class BuildingsController : Controller

{

    [HttpPost]

    public async Task<IActionResult> ToggleVisibility(int id)

    {

        var building = await _context.Buildings.FindAsync(id);

        building.IsHidden = !building.IsHidden;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

}

Kod 11. Przełączanie widoczności budynków

3.7.2. Zarządzanie lokalizacjami

Kontroler LocationsController oferuje:

-  Upload zdjęć (max 10 MB, formaty: jpg, png, gif, webp)
-  Upload plików audio (max 10 MB, formaty: mp3, wav, ogg, m4a)
-  Wybór typu lokalizacji (Room, Corridor, Hall, Stairs, Elevator, Entrance)
-  Określenie piętra

3.7.3. Zarządzanie przejściami

Kontroler TransitionsController umożliwia:

-  Definiowanie przejść między lokalizacjami
-  Wizualne pozycjonowanie strzałek (X%, Y%)
-  Oznaczanie dostępności dla wózków
-  Określanie kosztu przejścia (wpływa na algorytm nawigacji)

3.8. Dostępność (WCAG)

3.8.1. Wsparcie dla czytników ekranu

Aplikacja implementuje szereg rozwiązań dostępności:

cshtml

<!-- Tekst alternatywny dla obrazów -->

<img src="~/images/@Model.ImageFileName"

     alt="@imageAltText"

     aria-label="@imageAltText. Naciśnij Enter, aby przejść do opisu."

23

     tabindex="0" />

<!-- Nawigacja klawiaturą -->

<a class="nav-arrow"

   aria-label="Strzałka @directionName. Prowadzi do: @targetName."

   tabindex="0">

<!-- Ukryta treść dla czytników -->

<div class="visually-hidden" role="status" aria-live="polite">

    Aktualnie znajdujesz się w: @Model.Name

</div>

<!-- Oznaczenia dostępności -->

<span class="badge bg-success" aria-label="Dostępne dla wózków">

    ✓ Dostępne dla wózków

</span>

Kod 12. Elementy dostępności WCAG w widokach

Zaimplementowane standardy:

-  Atrybuty aria-label, aria-describedby, role
-  Klasa visually-hidden dla treści tylko dla czytników
-  Wsparcie nawigacji klawiaturą (Tab, Enter, Space)
-  Wyraźny fokus (outline) na elementach interaktywnych

4. Podsumowanie i wnioski

Celem  zrealizowanego  projektu  było  opracowanie  aplikacji  umożliwiającej  wirtualne
poruszanie  się  po  terenie  uczelni w sposób intuicyjny i zbliżony do rzeczywistego spaceru.
Zaprojektowane  rozwiązanie  spełnia  założone  wymagania  funkcjonalne  i  niefunkcjonalne,
łatwego  odnajdywania  budynków  oraz  sal  przy
oferując  użytkownikom  możliwość
wykorzystaniu  zdjęć  rzeczywistych  lokalizacji,  opisów  i  informacji  głosowych.  Aplikacja
charakteryzuje  się  prostą  obsługą,  responsywnym  interfejsem  oraz  możliwością  dalszej
rozbudowy.  Zastosowane  technologie  zapewniają  stabilność  działania  systemu  oraz
elastyczność  w  zakresie  przyszłych  modyfikacji.  Opracowane  rozwiązanie  może  stanowić
praktyczne  narzędzie  wspierające  orientację  przestrzenną  na  uczelni,  szczególnie  dla
nowych studentów oraz osób z niepełnosprawnościami.

Zrealizowane cele:

Interaktywny wirtualny spacer ze strzałkami kierunkowymi

-
-  System nawigacji z algorytmem Dijkstry
-  Wsparcie dla osób z niepełnosprawnościami (tryb winda, WCAG)

24

-  Panel administracyjny z pełnym zarządzaniem treścią
-  Responsywny interfejs (Bootstrap)
-  System uwierzytelniania i autoryzacji (ASP.NET Core Identity)

Możliwości rozwoju:

Integracja z mapami 3D lub panoramami 360°

-
-  Rozbudowa systemu o punkty POI (Point of Interest)
-  Aplikacja mobilna (PWA lub natywna)
-
Integracja z systemem rezerwacji sal
-  Wielojęzyczność interfejsu

Aplikacja  charakteryzuje  się  prostą  obsługą,  responsywnym  interfejsem  oraz  możliwością
dalszej rozbudowy. Zastosowane technologie zapewniają stabilność działania systemu oraz
elastyczność  w  zakresie  przyszłych  modyfikacji.  Opracowane  rozwiązanie  może  stanowić
praktyczne  narzędzie  wspierające  orientację  przestrzenną  na  uczelni,  szczególnie  dla
nowych studentów oraz osób z niepełnosprawnościami.

5.  Netografia i bibliografia

5.1. Netografia

●  Dokumentacja ASP.NET Core – https://learn.microsoft.com/aspnet/core
●  Dokumentacja Entity Framework Core – https://learn.microsoft.com/ef/core
●  Dokumentacja Microsoft SQL Server – https://learn.microsoft.com/sql
●  Dokumentacja Bootstrap – https://getbootstrap.com/docs
●  WCAG 2.1 – Web Content Accessibility Guidelines –
https://www.w3.org/WAI/standards-guidelines/wcag/

iGUIDE – https://iguide.com

●  Matterport – https://matterport.com
●
●  Metareal – https://www.metareal.com
●  Materiały udostępnione w kursie na platformie WLODEK

5.2. Bibliografia

●  UML @ Classroom (Martina Seidl, Marion Scholz, Gerti Kappel, Christian Huemer)
●  ASP.NET Core MVC 2. Zaawansowane programowanie. Wydanie VII (Adam

Freeman)

25

6. Spis tabel, spis rysunków, spis kodu

6.1. Rysunki

●  Rys.1. Persony użytkowników
●  Rys. 2. Diagram przypadków użycia (użytkownik)
●  Rys. 3. Diagram przypadków użycia (administrator)

6.2. Tabele

-  Tab. 1. Wykorzystane technologie i narzędzia
-  Tab. 2. Historia migracji bazy danych

6.3. Fragmenty kodu

-  Kod 1. Model Building (Budynek)
-  Kod 2. Model Location (Lokalizacja)
-  Kod 3. Model Transition (Przejście)
-  Kod 4. Model UserPreference (Preferencje użytkownika)
-  Kod 5. Kontekst bazy danych ApplicationDbContext
-  Kod 6. Seedowanie danych początkowych
-  Kod 7. Kontroler ExploreController – wyświetlanie lokalizacji
-  Kod 8. Algorytm Dijkstry w PathFinderService
-  Kod 9. Obsługa preferencji dostępności
-  Kod 10. Funkcja wyszukiwania lokalizacji
-  Kod 11. Przełączanie widoczności budynków
-  Kod 12. Elementy dostępności WCAG w widokach

26

