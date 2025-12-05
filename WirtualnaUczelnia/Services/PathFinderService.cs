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
    }

    public class PathFinderService
    {
        private readonly ApplicationDbContext _context;

        public PathFinderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NavigationStep>> FindPathAsync(int startId, int endId, string userId)
        {
            // 1. Sprawdź preferencje użytkownika (czy potrzebuje windy)
            bool avoidStairs = false;

            if (!string.IsNullOrEmpty(userId))
            {
                // Szukamy preferencji dla tego konkretnego UserID
                var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);

                // Jeśli znaleziono preferencję i jest zaznaczona niepełnosprawność -> unikaj schodów
                if (pref != null && pref.IsDisabled)
                {
                    avoidStairs = true;
                }
            }

            // 2. Pobierz wszystkie przejścia
            var allTransitions = await _context.Transitions
                .Include(t => t.TargetLocation)
                .ToListAsync();

            // 3. Algorytm BFS
            var queue = new Queue<int>();
            queue.Enqueue(startId);

            var cameFrom = new Dictionary<int, Transition>();
            var visited = new HashSet<int> { startId };

            bool found = false;

            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                if (currentId == endId)
                {
                    found = true;
                    break;
                }

                var neighbors = allTransitions.Where(t => t.SourceLocationId == currentId);

                foreach (var transition in neighbors)
                {
                    // FILTR: Jeśli unikamy schodów (avoidStairs=true), a przejście NIE jest dostępne (IsWheelchairAccessible=false) -> POMIŃ
                    if (avoidStairs && !transition.IsWheelchairAccessible)
                    {
                        continue;
                    }

                    if (!visited.Contains(transition.TargetLocationId))
                    {
                        visited.Add(transition.TargetLocationId);
                        cameFrom[transition.TargetLocationId] = transition;
                        queue.Enqueue(transition.TargetLocationId);
                    }
                }
            }

            // 4. Budowanie wyniku
            var path = new List<NavigationStep>();
            if (found)
            {
                var curr = endId;
                while (curr != startId)
                {
                    var trans = cameFrom[curr];
                    path.Add(new NavigationStep
                    {
                        Instruction = $"Idź {GetDirectionName(trans.Direction)} do: {trans.TargetLocation.Name}",
                        Icon = GetDirectionIcon(trans.Direction),
                        TargetLocationId = trans.TargetLocationId
                    });
                    curr = trans.SourceLocationId;
                }
                path.Reverse();
            }

            return path;
        }

        // Metody pomocnicze (bez zmian)
        private string GetDirectionName(Direction dir) => dir switch
        {
            Direction.Forward => "prosto",
            Direction.Back => "do tyłu",
            Direction.Left => "w lewo",
            Direction.Right => "w prawo",
            Direction.Up => "w górę (winda/schody)",
            Direction.Down => "w dół (winda/schody)",
            _ => "tam"
        };

        private string GetDirectionIcon(Direction dir) => dir switch
        {
            Direction.Forward => "⬆️",
            Direction.Back => "⬇️",
            Direction.Left => "⬅️",
            Direction.Right => "➡️",
            Direction.Up => "↗️",
            Direction.Down => "↘️",
            _ => "⏺️"
        };
    }
}