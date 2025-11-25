using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WirtualnaUczelnia.Models
{
    public enum Direction
    {
        [Display(Name = "Prosto")]
        Forward,
        [Display(Name = "W lewo")]
        Left,
        [Display(Name = "W prawo")]
        Right,
        [Display(Name = "W tył")]
        Back
    }

    public class Transition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Direction Direction { get; set; } // W którą stronę idziemy?

        // Skąd wychodzimy
        [ForeignKey("SourceLocation")]
        public int SourceLocationId { get; set; }
        public virtual Location SourceLocation { get; set; }

        // Dokąd dochodzimy
        [ForeignKey("TargetLocation")]
        public int TargetLocationId { get; set; }
        public virtual Location TargetLocation { get; set; }
    }
}