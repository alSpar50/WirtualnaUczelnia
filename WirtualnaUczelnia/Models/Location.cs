using System.ComponentModel.DataAnnotations;

namespace WirtualnaUczelnia.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa miejsca")]
        public string Name { get; set; } // np. "Wejście Główne", "Biblioteka"

        [Display(Name = "Opis")]
        public string Description { get; set; } // Krótki opis, co widać

        [Required]
        [Display(Name = "Plik zdjęcia")]
        public string ImageFileName { get; set; } // np. "hol_glowny.jpg"

        [Display(Name = "Plik audio")]
        public string AudioFileName { get; set; } // np. "hol_lektor.mp3"

        // Nawigacja - możliwe przejścia z tego miejsca
        public virtual ICollection<Transition> Transitions { get; set; }
    }
}