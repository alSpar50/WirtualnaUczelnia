# Skrypt migracji danych z SQL Server do SQLite
# Uruchom w PowerShell z katalogu WirtualnaUczelnia

Write-Host "=== MIGRACJA DANYCH Z SQL SERVER DO SQLITE ===" -ForegroundColor Cyan
Write-Host ""

$projectPath = Split-Path -Parent $MyInvocation.MyCommand.Path
if (-not $projectPath) { $projectPath = Get-Location }

$appSettingsPath = Join-Path $projectPath "appsettings.json"
$dataExportPath = Join-Path $projectPath "DataExport"

Write-Host "Sciezka projektu: $projectPath"
Write-Host ""

# Krok 1: Eksport danych z SQL Server
Write-Host "KROK 1: Eksport danych z SQL Server" -ForegroundColor Yellow
Write-Host "Upewnij sie, ze w appsettings.json masz: `"UseDatabase`": `"SqlServer`""
Write-Host ""
Write-Host "Uruchom aplikacje i przejdz do: /DataTools" -ForegroundColor Green
Write-Host "Kliknij 'Eksportuj dane'"
Write-Host ""
Read-Host "Nacisnij Enter gdy eksport zostanie zakonczony..."

# Sprawdz czy folder DataExport istnieje
if (-not (Test-Path $dataExportPath)) {
    Write-Host "BLAD: Folder DataExport nie istnieje! Wykonaj eksport najpierw." -ForegroundColor Red
    exit 1
}

$files = Get-ChildItem $dataExportPath -Filter "*.json"
if ($files.Count -eq 0) {
    Write-Host "BLAD: Brak plikow JSON w DataExport! Wykonaj eksport najpierw." -ForegroundColor Red
    exit 1
}

Write-Host "Znaleziono $($files.Count) plikow JSON do importu." -ForegroundColor Green
Write-Host ""

# Krok 2: Zmiana konfiguracji na SQLite
Write-Host "KROK 2: Zmiana konfiguracji na SQLite" -ForegroundColor Yellow

$appsettings = Get-Content $appSettingsPath -Raw | ConvertFrom-Json
$appsettings.UseDatabase = "Sqlite"
$appsettings | ConvertTo-Json -Depth 10 | Set-Content $appSettingsPath

Write-Host "Zmieniono UseDatabase na 'Sqlite' w appsettings.json" -ForegroundColor Green
Write-Host ""

# Krok 3: Usuñ star¹ bazê SQLite jeœli istnieje
$sqliteDbPath = Join-Path $projectPath "WirtualnaUczelnia.db"
if (Test-Path $sqliteDbPath) {
    Write-Host "Usuwanie starej bazy SQLite..." -ForegroundColor Yellow
    Remove-Item $sqliteDbPath -Force
}

Write-Host ""
Write-Host "KROK 3: Import danych do SQLite" -ForegroundColor Yellow
Write-Host "Uruchom aplikacje ponownie i przejdz do: /DataTools" -ForegroundColor Green
Write-Host "Kliknij 'Importuj dane'"
Write-Host ""
Read-Host "Nacisnij Enter gdy import zostanie zakonczony..."

# Sprawdz czy baza SQLite zosta³a utworzona
if (Test-Path $sqliteDbPath) {
    $size = (Get-Item $sqliteDbPath).Length / 1KB
    Write-Host "Baza SQLite utworzona: WirtualnaUczelnia.db ($([math]::Round($size, 2)) KB)" -ForegroundColor Green
} else {
    Write-Host "UWAGA: Plik bazy SQLite nie zostal znaleziony!" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== MIGRACJA ZAKONCZONA ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Teraz mozesz:"
Write-Host "1. Dodac plik WirtualnaUczelnia.db do repozytorium Git"
Write-Host "2. Scommitowac zmiany"
Write-Host "3. Inna osoba po sklonowaniu bedzie miala od razu dzialajaca aplikacje z danymi!"
Write-Host ""
Write-Host "Polecenia Git:" -ForegroundColor Yellow
Write-Host "  git add WirtualnaUczelnia/WirtualnaUczelnia.db"
Write-Host "  git add WirtualnaUczelnia/appsettings.json"
Write-Host "  git commit -m 'Migracja do SQLite z danymi'"
Write-Host "  git push"
