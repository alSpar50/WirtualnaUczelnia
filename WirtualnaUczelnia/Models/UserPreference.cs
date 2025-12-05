using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WirtualnaUczelnia.Models
{
    public class UserPreference
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } // Klucz obcy do tabeli użytkowników (string w Identity)

        [Display(Name = "Osoba z niepełnosprawnością (wymagana winda)")]
        public bool IsDisabled { get; set; }
    }
}
