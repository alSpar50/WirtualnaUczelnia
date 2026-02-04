# UZUPE£NIENIE DOKUMENTACJI - NOWE SEKCJE
# ==========================================
# Poni¿sze sekcje nale¿y dodaæ do dokumentacji projektu
# Ka¿da nowa sekcja jest oznaczona tagiem [NOWA SEKCJA]
# ==========================================

---

## [NOWA SEKCJA] 2.8.3. Scenariusze przypadków u¿ycia (User Stories)

Poni¿ej przedstawiono szczegó³owe scenariusze dla wybranych przypadków u¿ycia:

### Scenariusz 1: Wirtualny spacer po uczelni

| Element | Opis |
|---------|------|
| **Aktor** | Goœæ / U¿ytkownik zalogowany |
| **Cel** | Wirtualne zwiedzanie uczelni |
| **Warunki wstêpne** | U¿ytkownik znajduje siê na stronie g³ównej |
| **Przebieg g³ówny** | 1. U¿ytkownik wybiera budynek z listy kafelków<br>2. System wyœwietla pierwsz¹ lokalizacjê budynku<br>3. U¿ytkownik klika strza³kê kierunkow¹<br>4. System wyœwietla kolejn¹ lokalizacjê<br>5. U¿ytkownik kontynuuje nawigacjê |
| **Przebieg alternatywny** | 3a. U¿ytkownik u¿ywa listy przejœæ zamiast strza³ek<br>3b. U¿ytkownik odtwarza opis audio |
| **Warunki koñcowe** | U¿ytkownik zapozna³ siê z wybranymi lokalizacjami |

### Scenariusz 2: Wyszukanie trasy do sali

| Element | Opis |
|---------|------|
| **Aktor** | Goœæ / U¿ytkownik zalogowany |
| **Cel** | Znalezienie drogi do konkretnej sali |
| **Warunki wstêpne** | U¿ytkownik zna punkt startowy i docelowy |
| **Przebieg g³ówny** | 1. U¿ytkownik wybiera opcjê "ZnajdŸ drogê"<br>2. U¿ytkownik wybiera lokalizacjê startow¹<br>3. U¿ytkownik wybiera lokalizacjê docelow¹<br>4. (Opcjonalnie) U¿ytkownik zaznacza "Tryb winda"<br>5. System oblicza najkrótsz¹ trasê<br>6. System wyœwietla listê kroków nawigacji |
| **Przebieg alternatywny** | 5a. Brak mo¿liwej trasy - system informuje u¿ytkownika |
| **Warunki koñcowe** | U¿ytkownik otrzymuje instrukcje dotarcia do celu |

### Scenariusz 3: Zarz¹dzanie lokalizacjami (Administrator)

| Element | Opis |
|---------|------|
| **Aktor** | Administrator |
| **Cel** | Dodanie nowej lokalizacji do systemu |
| **Warunki wstêpne** | Administrator jest zalogowany |
| **Przebieg g³ówny** | 1. Administrator przechodzi do panelu lokalizacji<br>2. Klika "Dodaj now¹ lokalizacjê"<br>3. Wype³nia formularz (nazwa, opis, budynek, typ, piêtro)<br>4. Przesy³a zdjêcie i opcjonalnie plik audio<br>5. System waliduje dane i zapisuje lokalizacjê |
| **Przebieg alternatywny** | 5a. B³¹d walidacji - system wyœwietla komunikaty b³êdów |
| **Warunki koñcowe** | Nowa lokalizacja jest dostêpna w systemie |

### Scenariusz 4: Usuwanie budynku z kaskadowym usuniêciem powi¹zañ

| Element | Opis |
|---------|------|
| **Aktor** | Administrator |
| **Cel** | Usuniêcie budynku wraz z powi¹zanymi danymi |
| **Warunki wstêpne** | Administrator jest zalogowany, budynek istnieje |
| **Przebieg g³ówny** | 1. Administrator przechodzi do listy budynków<br>2. Klika "Usuñ" przy wybranym budynku<br>3. System wyœwietla ostrze¿enie z list¹ powi¹zanych lokalizacji i przejœæ<br>4. Administrator potwierdza usuniêcie<br>5. System usuwa wszystkie powi¹zane przejœcia<br>6. System usuwa wszystkie powi¹zane lokalizacje<br>7. System usuwa budynek |
| **Przebieg alternatywny** | 4a. Administrator anuluje operacjê - dane pozostaj¹ nienaruszone |
| **Warunki koñcowe** | Budynek i wszystkie powi¹zane dane zosta³y usuniête |

Tab. 3. Scenariusze przypadków u¿ycia

---

## [NOWA SEKCJA] 2.10. Uproszczony diagram ERD

Poni¿szy diagram przedstawia strukturê bazy danych aplikacji wraz z relacjami miêdzy encjami:

```
???????????????????????????????????????????????????????????????????????????????????
?                           DIAGRAM ERD - Wirtualna Uczelnia                       ?
???????????????????????????????????????????????????????????????????????????????????

    ?????????????????????????
    ?     AspNetUsers       ?
    ?    (Identity)         ?
    ?????????????????????????
    ? PK  Id (string)       ?
    ?     UserName          ?
    ?     Email             ?
    ?     PasswordHash      ?
    ?     ...               ?
    ?????????????????????????
                ?
                ? 1:1
                ?
    ?????????????????????????
    ?   UserPreference      ?
    ?????????????????????????
    ? PK  Id (int)          ?
    ? FK  UserId (string)   ?????????????????
    ?     IsDisabled (bool) ?               ?
    ?????????????????????????               ?
                                            ?
                                            ?
    ?????????????????????????               ?
    ?      Building         ?               ?
    ?????????????????????????               ?
    ? PK  Id (int)          ?               ?
    ?     Symbol (string)   ?               ?
    ?     Name (string)     ?               ?
    ?     Description       ?               ?
    ?     ImageFileName     ?               ?
    ?     ImageAltText      ?               ?
    ?     IsHidden (bool)   ?               ?
    ?????????????????????????               ?
                ?                           ?
                ? 1:N                       ?
                ?                           ?
    ?????????????????????????               ?
    ?      Location         ?               ?
    ?????????????????????????               ?
    ? PK  Id (int)          ?               ?
    ? FK  BuildingId (int?) ?????????????????
    ?     Name (string)     ?
    ?     Description       ?
    ?     ImageFileName     ?
    ?     ImageAltText      ?
    ?     AudioFileName     ?
    ?     IsHidden (bool)   ?
    ?     Type (enum)       ?
    ?     Floor (int)       ?
    ?????????????????????????
                ?
                ? 1:N (SourceLocation)
                ? 1:N (TargetLocation)
                ?
    ?????????????????????????
    ?     Transition        ?
    ?????????????????????????
    ? PK  Id (int)          ?
    ? FK  SourceLocationId  ?
    ? FK  TargetLocationId  ?
    ?     Direction (enum)  ?
    ?     PositionX (int)   ?
    ?     PositionY (int)   ?
    ?     IsWheelchairAcc.  ?
    ?     IsHidden (bool)   ?
    ?     Cost (int)        ?
    ?????????????????????????

LEGENDA:
?????????????????????????????
PK = Klucz g³ówny (Primary Key)
FK = Klucz obcy (Foreign Key)
1:1 = Relacja jeden do jednego
1:N = Relacja jeden do wielu

TYPY WYLICZENIOWE (ENUM):
?????????????????????????????
LocationType: Room, Corridor, Hall, Stairs, Elevator, Entrance, Other
Direction: Forward, Left, Right, Back, Up, Down
```

**Opis relacji:**

| Relacja | Typ | Opis |
|---------|-----|------|
| Building ? Location | 1:N | Jeden budynek zawiera wiele lokalizacji |
| Location ? Transition | 1:N | Jedna lokalizacja mo¿e byæ Ÿród³em wielu przejœæ |
| Location ? Transition | N:1 | Wiele przejœæ mo¿e prowadziæ do jednej lokalizacji |
| AspNetUsers ? UserPreference | 1:1 | Jeden u¿ytkownik ma jedne preferencje |

Tab. 4. Opis relacji w diagramie ERD

Rys. 4. Diagram ERD bazy danych (do wstawienia diagramu graficznego)

---

## [NOWA SEKCJA] 2.11. Schemat powi¹zañ miêdzy stronami aplikacji (Mapa aplikacji)

```
???????????????????????????????????????????????????????????????????????????????????
?                    MAPA APLIKACJI - Wirtualna Uczelnia                          ?
???????????????????????????????????????????????????????????????????????????????????

                              ???????????????????
                              ?   STRONA        ?
                              ?   G£ÓWNA        ?
                              ?   (Home/Index)  ?
                              ???????????????????
                                       ?
            ???????????????????????????????????????????????????????
            ?                          ?                          ?
            ?                          ?                          ?
    ?????????????????         ?????????????????         ?????????????????
    ?  WYSZUKIWARKA ?         ?   BUDYNKI     ?         ?   NAWIGACJA   ?
    ? (Home/Search) ?         ?  (kafelki)    ?         ?(Navigation/   ?
    ?????????????????         ?????????????????         ?    Index)     ?
            ?                         ?                 ?????????????????
            ?                         ?                         ?
            ?                         ?                         ?
    ?????????????????         ?????????????????         ?????????????????
    ?   WYNIKI      ?         ?   EXPLORE     ?         ?   WYNIK       ?
    ? WYSZUKIWANIA  ??????????? (Wirtualny    ?         ?   TRASY       ?
    ?               ?         ?   Spacer)     ?         ?(Navigation/   ?
    ?????????????????         ?????????????????         ?    Result)    ?
                                      ?                 ?????????????????
                                      ?
                              ?????????????????
                              ?   NAWIGACJA   ?
                              ?  STRZA£KAMI   ?
                              ?  (kolejne     ?
                              ?  lokalizacje) ?
                              ?????????????????


???????????????????????????????????????????????????????????????????????????????????
                           PANEL ADMINISTRACYJNY
                        (dostêpny tylko dla roli Admin)
???????????????????????????????????????????????????????????????????????????????????

                              ???????????????????
                              ?     ADMIN       ?
                              ?    (menu)       ?
                              ???????????????????
                                       ?
         ?????????????????????????????????????????????????????????????
         ?                             ?                             ?
         ?                             ?                             ?
???????????????????          ???????????????????          ???????????????????
?   BUDYNKI       ?          ?   LOKALIZACJE   ?          ?   PRZEJŒCIA     ?
? (Buildings)     ?          ?  (Locations)    ?          ? (Transitions)   ?
???????????????????          ???????????????????          ???????????????????
? • Index (lista) ?          ? • Index (lista) ?          ? • Index (lista) ?
? • Create        ?          ? • Create        ?          ? • Create        ?
? • Edit          ?          ? • Edit          ?          ? • Edit          ?
? • Delete        ?          ? • Delete        ?          ? • Delete        ?
? • Details       ?          ? • Details       ?          ? • Details       ?
? • Toggle        ?          ? • Toggle        ?          ? • Toggle        ?
?   Visibility    ?          ?   Visibility    ?          ?   Visibility    ?
???????????????????          ???????????????????          ???????????????????


???????????????????????????????????????????????????????????????????????????????????
                         SYSTEM UWIERZYTELNIANIA
                           (ASP.NET Identity)
???????????????????????????????????????????????????????????????????????????????????

    ?????????????????         ?????????????????         ?????????????????
    ?   LOGOWANIE   ?         ?  REJESTRACJA  ?         ?   ZARZ¥DZANIE ?
    ?    (Login)    ?         ?  (Register)   ?         ?    KONTEM     ?
    ?????????????????         ?????????????????         ?   (Manage)    ?
                                                        ?????????????????
                                                        ?• Profil       ?
                                                        ?• Has³o        ?
                                                        ?• Dostêpnoœæ   ?
                                                        ?  (preferencje)?
                                                        ?????????????????

LEGENDA:
?????????????????????????????
??? = Nawigacja/przejœcie miêdzy stronami
?   = Hierarchia/zawieranie
```

**Tabela nawigacji w aplikacji:**

| Strona Ÿród³owa | Strona docelowa | Akcja u¿ytkownika |
|-----------------|-----------------|-------------------|
| Strona g³ówna | Explore | Klikniêcie "WejdŸ do œrodka" na kafelku budynku |
| Strona g³ówna | Search | Wpisanie frazy i klikniêcie "Szukaj" |
| Strona g³ówna | Navigation | Klikniêcie "ZnajdŸ drogê" |
| Search | Explore | Klikniêcie "Zobacz" przy wyniku wyszukiwania |
| Search | Navigation | Klikniêcie "Wyznacz trasê" przy wyniku |
| Explore | Explore | Klikniêcie strza³ki nawigacyjnej |
| Navigation | Result | Wype³nienie formularza i klikniêcie "ZnajdŸ trasê" |
| Result | Explore | Klikniêcie "Podgl¹d" przy kroku trasy |

Tab. 5. Nawigacja miêdzy stronami aplikacji

Rys. 5. Mapa aplikacji - schemat powi¹zañ miêdzy stronami (do wstawienia diagramu graficznego)

---

## [NOWA SEKCJA] 3.9. Kontrolery aplikacji - przegl¹d

Aplikacja wykorzystuje nastêpuj¹ce kontrolery:

### 3.9.1. HomeController

Kontroler obs³uguj¹cy stronê g³ówn¹ i wyszukiwarkê.

| Akcja | Metoda HTTP | Opis |
|-------|-------------|------|
| Index | GET | Wyœwietla listê budynków w formie kafelków |
| Search | GET | Wyszukuje lokalizacje po nazwie i opisie |
| Privacy | GET | Wyœwietla politykê prywatnoœci |

### 3.9.2. ExploreController

Kontroler obs³uguj¹cy wirtualny spacer.

| Akcja | Metoda HTTP | Opis |
|-------|-------------|------|
| Index | GET | Wyœwietla lokalizacjê z przejœciami (strza³kami) |

### 3.9.3. NavigationController

Kontroler obs³uguj¹cy system nawigacji i wyznaczania tras.

| Akcja | Metoda HTTP | Opis |
|-------|-------------|------|
| Index | GET | Wyœwietla formularz wyboru trasy |
| Result | GET | Oblicza i wyœwietla wyznaczon¹ trasê |

### 3.9.4. BuildingsController (Admin)

Kontroler CRUD dla budynków (wymaga roli Admin).

| Akcja | Metoda HTTP | Opis |
|-------|-------------|------|
| Index | GET | Lista wszystkich budynków |
| Details | GET | Szczegó³y budynku |
| Create | GET/POST | Tworzenie nowego budynku |
| Edit | GET/POST | Edycja budynku |
| Delete | GET/POST | Usuwanie budynku z kaskadowym usuniêciem powi¹zañ |
| ToggleVisibility | POST | Prze³¹czanie widocznoœci budynku |

### 3.9.5. LocationsController (Admin)

Kontroler CRUD dla lokalizacji (wymaga roli Admin).

| Akcja | Metoda HTTP | Opis |
|-------|-------------|------|
| Index | GET | Lista wszystkich lokalizacji |
| Details | GET | Szczegó³y lokalizacji |
| Create | GET/POST | Tworzenie lokalizacji z uploadem plików |
| Edit | GET/POST | Edycja lokalizacji |
| Delete | GET/POST | Usuwanie lokalizacji z kaskadowym usuniêciem przejœæ |
| ToggleVisibility | POST | Prze³¹czanie widocznoœci lokalizacji |

### 3.9.6. TransitionsController (Admin)

Kontroler CRUD dla przejœæ miêdzy lokalizacjami (wymaga roli Admin).

| Akcja | Metoda HTTP | Opis |
|-------|-------------|------|
| Index | GET | Lista wszystkich przejœæ |
| Create | GET/POST | Tworzenie przejœcia z podgl¹dem lokalizacji |
| Edit | GET/POST | Edycja przejœcia |
| Delete | GET/POST | Usuwanie przejœcia |
| ToggleVisibility | POST | Prze³¹czanie widocznoœci przejœcia |

Tab. 6. Przegl¹d kontrolerów aplikacji

---

## [NOWA SEKCJA] 3.10. Widoki aplikacji - przegl¹d

### Struktura katalogów Views:

```
Views/
??? Home/
?   ??? Index.cshtml          - Strona g³ówna z kafelkami budynków
?   ??? Search.cshtml         - Wyniki wyszukiwania
?   ??? Privacy.cshtml        - Polityka prywatnoœci
??? Explore/
?   ??? Index.cshtml          - Widok wirtualnego spaceru
??? Navigation/
?   ??? Index.cshtml          - Formularz wyboru trasy
?   ??? Result.cshtml         - Wynik nawigacji (lista kroków)
??? Buildings/
?   ??? Index.cshtml          - Lista budynków (admin)
?   ??? Create.cshtml         - Formularz tworzenia
?   ??? Edit.cshtml           - Formularz edycji
?   ??? Delete.cshtml         - Potwierdzenie usuniêcia
?   ??? Details.cshtml        - Szczegó³y budynku
??? Locations/
?   ??? Index.cshtml          - Lista lokalizacji (admin)
?   ??? Create.cshtml         - Formularz tworzenia
?   ??? Edit.cshtml           - Formularz edycji
?   ??? Delete.cshtml         - Potwierdzenie usuniêcia
?   ??? Details.cshtml        - Szczegó³y lokalizacji
??? Transitions/
?   ??? Index.cshtml          - Lista przejœæ (admin)
?   ??? Create.cshtml         - Formularz tworzenia
?   ??? Edit.cshtml           - Formularz edycji
?   ??? Delete.cshtml         - Potwierdzenie usuniêcia
?   ??? Details.cshtml        - Szczegó³y przejœcia
??? Shared/
    ??? _Layout.cshtml        - G³ówny layout strony
    ??? _LoginPartial.cshtml  - Partial logowania
    ??? Error.cshtml          - Strona b³êdów
```

Tab. 7. Struktura widoków aplikacji

---

## [NOWA SEKCJA] 3.11. Funkcjonowanie aplikacji - szczegó³owy opis z ilustracjami

### 3.11.1. Strona g³ówna (Home/Index)

**Opis funkcji:**
Strona g³ówna prezentuje listê dostêpnych budynków uczelni w formie interaktywnych kafelków. Ka¿dy kafelek zawiera zdjêcie budynku, jego symbol, nazwê, skrócony opis oraz przycisk umo¿liwiaj¹cy rozpoczêcie wirtualnego spaceru. U góry strony znajduje siê wyszukiwarka pozwalaj¹ca na szybkie odnalezienie konkretnego miejsca.

**Fragment kodu (HomeController.cs):**

```csharp
public async Task<IActionResult> Index()
{
    var buildings = await _context.Buildings
        .Where(b => !b.IsHidden)
        .OrderBy(b => b.Symbol)
        .ToListAsync();
    
    return View(buildings);
}
```

Kod 13. Akcja Index w HomeController

Rys. 6. Widok strony g³ównej aplikacji (do wstawienia zrzutu ekranu)

---

### 3.11.2. Wyszukiwarka lokalizacji (Home/Search)

**Opis funkcji:**
Wyszukiwarka przeszukuje nazwy i opisy wszystkich widocznych lokalizacji w bazie danych. Wyniki s¹ prezentowane w formie listy z informacj¹ o budynku, w którym znajduje siê dana lokalizacja, oraz z mo¿liwoœci¹ przejœcia do wirtualnego spaceru lub wyznaczenia trasy.

**Fragment kodu (HomeController.cs):**

```csharp
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
```

Kod 14. Funkcja wyszukiwania w HomeController

Rys. 7. Widok wyników wyszukiwania (do wstawienia zrzutu ekranu)

---

### 3.11.3. Wirtualny spacer (Explore/Index)

**Opis funkcji:**
G³ówny modu³ aplikacji prezentuj¹cy zdjêcie aktualnej lokalizacji z na³o¿onymi interaktywnymi strza³kami nawigacyjnymi. Strza³ki s¹ pozycjonowane procentowo na zdjêciu zgodnie z konfiguracj¹ w bazie danych. U¿ytkownik mo¿e przemieszczaæ siê miêdzy lokalizacjami klikaj¹c strza³ki lub wybieraj¹c przejœcie z rozwijanej listy. Dla ka¿dej lokalizacji dostêpny jest opis tekstowy oraz opcjonalny opis audio.

**Fragment kodu widoku (Explore/Index.cshtml):**

```cshtml
<div class="arrows-overlay" role="navigation" 
     aria-label="Nawigacja po lokalizacjach - strza³ki kierunkowe">
    @foreach (var transition in Model.Transitions)
    {
        <a asp-action="Index" asp-route-id="@transition.TargetLocationId"
           class="nav-arrow"
           style="left: @transition.PositionX%; top: @transition.PositionY%;"
           aria-label="Strza³ka @directionName. Prowadzi do: @targetName."
           tabindex="0" role="link">
            <span aria-hidden="true">@arrowIcon</span>
        </a>
    }
</div>
```

Kod 15. Renderowanie strza³ek nawigacyjnych w widoku Explore

Rys. 8. Widok wirtualnego spaceru ze strza³kami nawigacyjnymi (do wstawienia zrzutu ekranu)

---

### 3.11.4. Formularz nawigacji (Navigation/Index)

**Opis funkcji:**
Formularz umo¿liwia u¿ytkownikowi wybór lokalizacji startowej i docelowej z list rozwijanych. Dla niezalogowanych u¿ytkowników dostêpny jest checkbox "Tryb winda", który wymusza wyznaczanie tras dostêpnych dla osób z niepe³nosprawnoœciami. Zalogowani u¿ytkownicy mog¹ zapisaæ preferencje dostêpnoœci w swoim profilu.

**Fragment kodu (NavigationController.cs):**

```csharp
public async Task<IActionResult> Index()
{
    var locations = await _context.Locations
        .Include(l => l.Building)
        .Where(l => !l.IsHidden)
        .Where(l => l.Building == null || !l.Building.IsHidden)
        .OrderBy(l => l.Building.Symbol)
        .ThenBy(l => l.Name)
        .ToListAsync();

    ViewBag.Locations = new SelectList(locations, "Id", "Name");
    
    // Odczyt preferencji z sesji dla niezalogowanych
    var savedMode = HttpContext.Session.GetString(WheelchairModeSessionKey);
    ViewBag.WheelchairMode = savedMode == "true";
    
    return View();
}
```

Kod 16. Akcja Index w NavigationController

Rys. 9. Formularz wyboru trasy (do wstawienia zrzutu ekranu)

---

### 3.11.5. Wynik nawigacji (Navigation/Result)

**Opis funkcji:**
Prezentacja znalezionej trasy w formie numerowanej listy kroków. Ka¿dy krok zawiera ikonê kierunku (strza³kê), instrukcjê tekstow¹ opisuj¹c¹ ruch, oraz przycisk "Podgl¹d" wyœwietlaj¹cy miniaturê zdjêcia lokalizacji w tooltipie. Ostatni krok jest oznaczony jako cel podró¿y.

**Fragment kodu widoku (Navigation/Result.cshtml):**

```cshtml
@foreach (var step in Model)
{
    stepNumber++;
    <li class="list-group-item d-flex align-items-center">
        <span class="badge bg-primary rounded-pill me-3">@stepNumber</span>
        <span class="me-2 fs-4" aria-hidden="true">@step.Icon</span>
        <span class="flex-grow-1">@step.Instruction</span>
        
        <button type="button" class="btn btn-sm btn-outline-secondary"
                data-bs-toggle="tooltip" data-bs-html="true"
                title="<img src='/images/@step.ImageFileName' style='max-width:200px;'/>">
            <i class="bi bi-eye"></i> Podgl¹d
        </button>
    </li>
}
```

Kod 17. Wyœwietlanie kroków nawigacji

Rys. 10. Wynik nawigacji - lista kroków trasy (do wstawienia zrzutu ekranu)

---

### 3.11.6. Panel administracyjny - Zarz¹dzanie budynkami

**Opis funkcji:**
Panel administracyjny umo¿liwia zarz¹dzanie budynkami w systemie. Administrator mo¿e dodawaæ nowe budynki, edytowaæ istniej¹ce, prze³¹czaæ ich widocznoœæ (ukrywaæ przed u¿ytkownikami) oraz usuwaæ. Przy usuwaniu budynku system wyœwietla ostrze¿enie z list¹ powi¹zanych lokalizacji i przejœæ, które zostan¹ równie¿ usuniête.

**Fragment kodu (BuildingsController.cs):**

```csharp
// GET: Buildings/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null) return NotFound();

    var building = await _context.Buildings
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.Id == id);
    if (building == null) return NotFound();

    // Pobierz powi¹zane lokacje
    var relatedLocations = await _context.Locations
        .Where(l => l.BuildingId == id)
        .ToListAsync();

    // Pobierz powi¹zane przejœcia (przez lokacje)
    var locationIds = relatedLocations.Select(l => l.Id).ToList();
    var relatedTransitions = await _context.Transitions
        .Include(t => t.SourceLocation)
        .Include(t => t.TargetLocation)
        .Where(t => locationIds.Contains(t.SourceLocationId) || 
                    locationIds.Contains(t.TargetLocationId))
        .ToListAsync();

    ViewBag.RelatedLocations = relatedLocations;
    ViewBag.RelatedTransitions = relatedTransitions;
    ViewBag.HasRelatedData = relatedLocations.Any() || relatedTransitions.Any();

    return View(building);
}
```

Kod 18. Usuwanie budynku z informacj¹ o powi¹zanych danych

Rys. 11. Lista budynków w panelu administracyjnym (do wstawienia zrzutu ekranu)

Rys. 12. Widok usuwania budynku z ostrze¿eniem o powi¹zanych danych (do wstawienia zrzutu ekranu)

---

### 3.11.7. Panel administracyjny - Zarz¹dzanie lokalizacjami

**Opis funkcji:**
Panel umo¿liwia zarz¹dzanie lokalizacjami (salami, korytarzami, windami itp.). Administrator mo¿e dodawaæ zdjêcia oraz pliki audio dla ka¿dej lokalizacji. System waliduje przesy³ane pliki (maksymalny rozmiar 10 MB, dozwolone rozszerzenia). Przy usuwaniu lokalizacji wyœwietlane jest ostrze¿enie o powi¹zanych przejœciach.

**Fragment kodu (LocationsController.cs):**

```csharp
// Walidacja pliku obrazu
private string? ValidateFile(IFormFile file, string[] allowedExtensions, string fileTypeName)
{
    if (file.Length > MaxFileSize)
    {
        return $"Plik {fileTypeName} jest za du¿y. Maksymalny rozmiar to {MaxFileSize / 1024 / 1024} MB.";
    }

    var extension = Path.GetExtension(file.FileName).ToLower();
    if (!allowedExtensions.Contains(extension))
    {
        return $"Niedozwolone rozszerzenie pliku {fileTypeName}. Dozwolone: {string.Join(", ", allowedExtensions)}";
    }

    return null;
}
```

Kod 19. Walidacja przesy³anych plików

Rys. 13. Formularz dodawania lokalizacji (do wstawienia zrzutu ekranu)

Rys. 14. Widok usuwania lokalizacji z list¹ powi¹zanych przejœæ (do wstawienia zrzutu ekranu)

---

### 3.11.8. Panel administracyjny - Zarz¹dzanie przejœciami

**Opis funkcji:**
Panel umo¿liwia definiowanie przejœæ (strza³ek) miêdzy lokalizacjami. Administrator wybiera lokalizacjê Ÿród³ow¹ i docelow¹, kierunek przejœcia, pozycjê strza³ki na zdjêciu (X%, Y%), koszt przejœcia oraz dostêpnoœæ dla wózków. Formularz zawiera podgl¹d zdjêæ obu lokalizacji.

Rys. 15. Formularz tworzenia przejœcia z podgl¹dem lokalizacji (do wstawienia zrzutu ekranu)

Rys. 16. Lista przejœæ w panelu administracyjnym (do wstawienia zrzutu ekranu)

---

### 3.11.9. Ustawienia dostêpnoœci (Identity/Manage/Accessibility)

**Opis funkcji:**
Zalogowani u¿ytkownicy mog¹ zapisaæ swoje preferencje dotycz¹ce dostêpnoœci. Opcja "Osoba z niepe³nosprawnoœci¹" powoduje automatyczne wyznaczanie tras dostêpnych dla wózków (wykluczenie schodów, preferowanie wind).

Rys. 17. Strona ustawieñ dostêpnoœci w profilu u¿ytkownika (do wstawienia zrzutu ekranu)

---

## [NOWA SEKCJA] Aktualizacja spisu

### Spis rysunków (uzupe³nienie):

- Rys. 4. Diagram ERD bazy danych
- Rys. 5. Mapa aplikacji - schemat powi¹zañ miêdzy stronami
- Rys. 6. Widok strony g³ównej aplikacji
- Rys. 7. Widok wyników wyszukiwania
- Rys. 8. Widok wirtualnego spaceru ze strza³kami nawigacyjnymi
- Rys. 9. Formularz wyboru trasy
- Rys. 10. Wynik nawigacji - lista kroków trasy
- Rys. 11. Lista budynków w panelu administracyjnym
- Rys. 12. Widok usuwania budynku z ostrze¿eniem o powi¹zanych danych
- Rys. 13. Formularz dodawania lokalizacji
- Rys. 14. Widok usuwania lokalizacji z list¹ powi¹zanych przejœæ
- Rys. 15. Formularz tworzenia przejœcia z podgl¹dem lokalizacji
- Rys. 16. Lista przejœæ w panelu administracyjnym
- Rys. 17. Strona ustawieñ dostêpnoœci w profilu u¿ytkownika

### Spis tabel (uzupe³nienie):

- Tab. 3. Scenariusze przypadków u¿ycia
- Tab. 4. Opis relacji w diagramie ERD
- Tab. 5. Nawigacja miêdzy stronami aplikacji
- Tab. 6. Przegl¹d kontrolerów aplikacji
- Tab. 7. Struktura widoków aplikacji

### Spis kodu (uzupe³nienie):

- Kod 13. Akcja Index w HomeController
- Kod 14. Funkcja wyszukiwania w HomeController
- Kod 15. Renderowanie strza³ek nawigacyjnych w widoku Explore
- Kod 16. Akcja Index w NavigationController
- Kod 17. Wyœwietlanie kroków nawigacji
- Kod 18. Usuwanie budynku z informacj¹ o powi¹zanych danych
- Kod 19. Walidacja przesy³anych plików

---

# KONIEC UZUPE£NIENIA DOKUMENTACJI
