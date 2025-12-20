using System.ComponentModel.DataAnnotations;

namespace WirtualnaUczelnia.Models
{
    /// <summary>
    /// Typ lokalizacji - pomaga w nawigacji i wyświetlaniu
    /// </summary>
    public enum LocationType
    {
        [Display(Name = "Pomieszczenie")] Room,           // Sala, biuro, sekretariat - cel podróży
        [Display(Name = "Korytarz")] Corridor,            // Korytarz - przez który się przechodzi
        [Display(Name = "Hol")] Hall,                     // Hol główny, lobby
        [Display(Name = "Schody")] Stairs,                // Klatka schodowa
        [Display(Name = "Winda")] Elevator,               // Winda
        [Display(Name = "Wejście")] Entrance,             // Wejście do budynku
        [Display(Name = "Inne")] Other                    // Inne miejsca
    }

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

        [Display(Name = "Tekst alternatywny (dla czytników)")]
        [StringLength(500)]
        public string? ImageAltText { get; set; } // Tekst alternatywny dla NVDA i innych czytników

        [Display(Name = "Plik audio")]
        public string? AudioFileName { get; set; } // np. "hol_lektor.mp3"

        [Display(Name = "Ukryta")]
        public bool IsHidden { get; set; } = false; // Nowe pole - domyślnie widoczna

        /// <summary>
        /// Typ lokalizacji - pomaga określić czy to cel podróży czy punkt pośredni
        /// </summary>
        [Display(Name = "Typ lokalizacji")]
        public LocationType Type { get; set; } = LocationType.Room;

        /// <summary>
        /// Numer piętra (0 = parter, 1 = pierwsze piętro, -1 = piwnica)
        /// Pomocne przy nawigacji pionowej
        /// </summary>
        [Display(Name = "Piętro")]
        public int Floor { get; set; } = 0;

        // Nawigacja - możliwe przejścia z tego miejsca
        public virtual ICollection<Transition> Transitions { get; set; }

        // Relacja do Budynku (Opcjonalna, bo może być np. "Dziedziniec" nieprzypisany do budynku)
        [Display(Name = "Budynek")]
        public int? BuildingId { get; set; }

        public virtual Building? Building { get; set; }
    }
}