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
        public string? LocationType { get; set; }  // Typ docelowej lokacji
        public int? Floor { get; set; }            // Piętro docelowe
    }

    public class PathFinderService
    {
        private readonly ApplicationDbContext _context;

        // Mnożnik kosztu dla schodów gdy użytkownik potrzebuje dostępności
        // (wysoka wartość sprawia, że schody są bardzo nieopłacalne)
        private const int STAIRS_PENALTY_FOR_DISABLED = 10000;

        public PathFinderService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Znajduje najkrótszą ścieżkę używając algorytmu Dijkstry z wagami (kosztami przejść).
        /// Dla osób niepełnosprawnych schody mają bardzo wysoki koszt (praktycznie niedostępne).
        /// </summary>
        public async Task<List<NavigationStep>> FindPathAsync(int startId, int endId, string userId)
        {
            // 1. Sprawdź preferencje użytkownika
            bool avoidStairs = false;

            if (!string.IsNullOrEmpty(userId))
            {
                var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
                if (pref != null && pref.IsDisabled)
                {
                    avoidStairs = true;
                }
            }

            // 2. Pobierz wszystkie widoczne przejścia
            var allTransitions = await _context.Transitions
                .Include(t => t.TargetLocation)
                    .ThenInclude(l => l.Building)
                .Include(t => t.SourceLocation)
                    .ThenInclude(l => l.Building)
                .Where(t => !t.IsHidden)
                .Where(t => !t.SourceLocation.IsHidden)
                .Where(t => !t.TargetLocation.IsHidden)
                .Where(t => t.SourceLocation.Building == null || !t.SourceLocation.Building.IsHidden)
                .Where(t => t.TargetLocation.Building == null || !t.TargetLocation.Building.IsHidden)
                .ToListAsync();

            // 3. Budujemy graf jako słownik: SourceId -> Lista (TargetId, Transition, EffectiveCost)
            var graph = new Dictionary<int, List<(int targetId, Transition transition, int cost)>>();
            
            foreach (var t in allTransitions)
            {
                if (!graph.ContainsKey(t.SourceLocationId))
                    graph[t.SourceLocationId] = new List<(int, Transition, int)>();

                // Oblicz efektywny koszt
                int effectiveCost = t.Cost;
                
                // Jeśli użytkownik unika schodów, a przejście nie jest dostępne dla wózków
                if (avoidStairs && !t.IsWheelchairAccessible)
                {
                    effectiveCost += STAIRS_PENALTY_FOR_DISABLED; // Ogromna kara - schody będą ostatecznością
                }

                graph[t.SourceLocationId].Add((t.TargetLocationId, t, effectiveCost));
            }

            // 4. Algorytm Dijkstry
            var distances = new Dictionary<int, int>();
            var cameFrom = new Dictionary<int, Transition>();
            var visited = new HashSet<int>();

            // PriorityQueue: (koszt, locationId) - sortuje od najmniejszego kosztu
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

            // 5. Budowanie wyniku (odtworzenie ścieżki)
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
                        Floor = trans.TargetLocation.Floor
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