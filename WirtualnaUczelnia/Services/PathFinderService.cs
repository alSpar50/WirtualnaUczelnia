using Microsoft.EntityFrameworkCore;
using WirtualnaUczelnia.Data;
using WirtualnaUczelnia.Models;

namespace WirtualnaUczelnia.Services
{
    public class NavigationStep
    {
        public string Instruction { get; set; }
        public string Icon { get; set; }
        public int TargetLocationId { get; set; }
        public string? LocationType { get; set; }
        public int? Floor { get; set; }
        public string? ImageFileName { get; set; }      // NOWE: nazwa pliku zdjęcia
        public string? ImageAltText { get; set; }       // NOWE: tekst alternatywny
        public string? LocationName { get; set; }       // NOWE: nazwa lokacji
    }

    public class PathFinderService
    {
        private readonly ApplicationDbContext _context;

        // Kara dla windy gdy użytkownik jest pełnosprawny (schody są szybsze)
        private const int ELEVATOR_PENALTY_FOR_ABLED = 15;

        public PathFinderService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Znajduje najkrótszą ścieżkę używając algorytmu Dijkstry.
        /// Wersja dla zalogowanych użytkowników - pobiera preferencje z bazy.
        /// </summary>
        public async Task<List<NavigationStep>> FindPathAsync(int startId, int endId, string? userId)
        {
            // Sprawdź preferencje użytkownika z bazy
            bool requireWheelchairAccess = false;

            if (!string.IsNullOrEmpty(userId))
            {
                var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
                if (pref != null && pref.IsDisabled)
                {
                    requireWheelchairAccess = true;
                }
            }

            return await FindPathAsync(startId, endId, requireWheelchairAccess);
        }

        /// <summary>
        /// Znajduje najkrótszą ścieżkę używając algorytmu Dijkstry.
        /// Wersja z bezpośrednim parametrem dostępności - dla niezalogowanych użytkowników.
        /// - Dla osób na wózku: przejścia niedostępne są CAŁKOWICIE WYKLUCZONE
        /// - Dla osób pełnosprawnych: winda ma karę (schody preferowane)
        /// </summary>
        public async Task<List<NavigationStep>> FindPathAsync(int startId, int endId, bool requireWheelchairAccess)
        {
            // 1. Pobierz wszystkie widoczne przejścia
            var query = _context.Transitions
                .Include(t => t.TargetLocation)
                    .ThenInclude(l => l.Building)
                .Include(t => t.SourceLocation)
                    .ThenInclude(l => l.Building)
                .Where(t => !t.IsHidden)
                .Where(t => !t.SourceLocation.IsHidden)
                .Where(t => !t.TargetLocation.IsHidden)
                .Where(t => t.SourceLocation.Building == null || !t.SourceLocation.Building.IsHidden)
                .Where(t => t.TargetLocation.Building == null || !t.TargetLocation.Building.IsHidden);

            // Dla osób na wózku - CAŁKOWICIE WYKLUCZ przejścia niedostępne
            if (requireWheelchairAccess)
            {
                query = query.Where(t => t.IsWheelchairAccessible);
            }

            var allTransitions = await query.ToListAsync();

            // 2. Budujemy graf: SourceId -> Lista (TargetId, Transition, Cost)
            var graph = new Dictionary<int, List<(int targetId, Transition transition, int cost)>>();
            
            foreach (var t in allTransitions)
            {
                if (!graph.ContainsKey(t.SourceLocationId))
                    graph[t.SourceLocationId] = new List<(int, Transition, int)>();

                int effectiveCost = t.Cost > 0 ? t.Cost : 10; // Domyślny koszt 10 jeśli nie ustawiono
                
                // Dla osób PEŁNOSPRAWNYCH - dodaj karę dla windy (schody są szybsze)
                if (!requireWheelchairAccess)
                {
                    // Jeśli przejście prowadzi do windy - dodaj karę
                    if (t.TargetLocation.Type == LocationType.Elevator)
                    {
                        effectiveCost += ELEVATOR_PENALTY_FOR_ABLED;
                    }
                    
                    // Jeśli przejście to schody (niedostępne dla wózków) - daj bonus
                    if (!t.IsWheelchairAccessible)
                    {
                        effectiveCost = Math.Max(1, effectiveCost - 5);
                    }
                }

                graph[t.SourceLocationId].Add((t.TargetLocationId, t, effectiveCost));
            }

            // 3. Algorytm Dijkstry
            var distances = new Dictionary<int, int>();
            var cameFrom = new Dictionary<int, Transition>();
            var visited = new HashSet<int>();
            var priorityQueue = new PriorityQueue<int, int>();

            distances[startId] = 0;
            priorityQueue.Enqueue(startId, 0);

            while (priorityQueue.Count > 0)
            {
                var currentId = priorityQueue.Dequeue();

                // Znaleźliśmy cel
                if (currentId == endId)
                    break;

                // Już odwiedzony z lepszym kosztem
                if (visited.Contains(currentId))
                    continue;

                visited.Add(currentId);

                // Sprawdź sąsiadów
                if (graph.ContainsKey(currentId))
                {
                    foreach (var (targetId, transition, cost) in graph[currentId])
                    {
                        if (visited.Contains(targetId))
                            continue;

                        int newDist = distances[currentId] + cost;

                        // Jeśli znaleźliśmy krótszą ścieżkę
                        if (!distances.ContainsKey(targetId) || newDist < distances[targetId])
                        {
                            distances[targetId] = newDist;
                            cameFrom[targetId] = transition;
                            priorityQueue.Enqueue(targetId, newDist);
                        }
                    }
                }
            }

            // 4. Budowanie wyniku
            var path = new List<NavigationStep>();
            
            if (distances.ContainsKey(endId))
            {
                var curr = endId;
                while (curr != startId && cameFrom.ContainsKey(curr))
                {
                    var trans = cameFrom[curr];
                    path.Add(new NavigationStep
                    {
                        Instruction = BuildInstruction(trans),
                        Icon = GetDirectionIcon(trans.Direction),
                        TargetLocationId = trans.TargetLocationId,
                        LocationType = trans.TargetLocation.Type.ToString(),
                        Floor = trans.TargetLocation.Floor,
                        ImageFileName = trans.TargetLocation.ImageFileName,
                        ImageAltText = trans.TargetLocation.ImageAltText,
                        LocationName = trans.TargetLocation.Name
                    });
                    curr = trans.SourceLocationId;
                }
                path.Reverse();
            }

            return path;
        }

        /// <summary>
        /// Buduje czytelną instrukcję nawigacyjną
        /// </summary>
        private string BuildInstruction(Transition trans)
        {
            var target = trans.TargetLocation;
            string directionText = GetDirectionName(trans.Direction);
            
            // Dostosuj instrukcję w zależności od typu lokacji docelowej
            return target.Type switch
            {
                LocationType.Stairs => $"Idź {directionText} do klatki schodowej: {target.Name}",
                LocationType.Elevator => $"Idź {directionText} do windy: {target.Name}",
                LocationType.Corridor => $"Idź {directionText} korytarzem: {target.Name}",
                LocationType.Hall => $"Idź {directionText} przez hol: {target.Name}",
                LocationType.Entrance => $"Idź {directionText} do wejścia: {target.Name}",
                LocationType.Room => $"Idź {directionText} do: {target.Name}",
                _ => $"Idź {directionText} do: {target.Name}"
            };
        }

        private string GetDirectionName(Direction dir) => dir switch
        {
            Direction.Forward => "prosto",
            Direction.Back => "do tyłu",
            Direction.Left => "w lewo",
            Direction.Right => "w prawo",
            Direction.Up => "w górę",
            Direction.Down => "w dół",
            _ => "tam"
        };

        private string GetDirectionIcon(Direction dir) => dir switch
        {
            Direction.Forward => "⬆️",
            Direction.Back => "⬇️",
            Direction.Left => "⬅️",
            Direction.Right => "➡️",
            Direction.Up => "🔼",
            Direction.Down => "🔽",
            _ => "⏺️"
        };
    }
}