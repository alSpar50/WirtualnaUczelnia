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

        [ForeignKey("SourceLocation")]
        public int SourceLocationId { get; set; }
        public virtual Location SourceLocation { get; set; }

        [ForeignKey("TargetLocation")]
        public int TargetLocationId { get; set; }
        public virtual Location TargetLocation { get; set; }
    }
}