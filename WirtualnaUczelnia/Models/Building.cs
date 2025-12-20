using System.ComponentModel.DataAnnotations;

namespace WirtualnaUczelnia.Models
{
    public class Building
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Symbol budynku")]
        public string Symbol { get; set; } // np. "A", "B", "H"

        [Display(Name = "Pełna nazwa")]
        public string Name { get; set; } // np. "Centrum Sportowo-Rekreacyjne"

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Zdjęcie budynku")]
        public string? ImageFileName { get; set; } // <--- NOWE POLE

        [Display(Name = "Tekst alternatywny (dla czytników)")]
        [StringLength(500)]
        public string? ImageAltText { get; set; } // Tekst alternatywny dla NVDA i innych czytników

        [Display(Name = "Ukryty")]
        public bool IsHidden { get; set; } = false; // Nowe pole - domyślnie widoczny

        // Lista lokacji w tym budynku
        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
