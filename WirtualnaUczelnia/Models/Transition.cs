using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WirtualnaUczelnia.Models
{
    public enum Direction
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

        [Display(Name = "Dostępne dla wózków (winda/płasko)")]
        public bool IsWheelchairAccessible { get; set; } = true; // Domyślnie tak

        [Display(Name = "Ukryte")]
        public bool IsHidden { get; set; } = false; // Nowe pole - domyślnie widoczne

        /// <summary>
        /// Koszt/odległość przejścia w umownych jednostkach (np. sekundy, metry).
        /// Im wyższy koszt, tym mniej preferowane przejście.
        /// Domyślnie 10 = standardowe przejście przez korytarz.
        /// Schody mogą mieć wyższy koszt (np. 20), winda jeszcze wyższy (np. 30 - bo trzeba czekać).
        /// </summary>
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
}