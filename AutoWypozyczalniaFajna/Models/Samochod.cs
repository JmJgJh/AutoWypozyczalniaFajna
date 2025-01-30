using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoWypozyczalniaFajna.Models
{
    public class Samochod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Marka")]
        public int MarkaId { get; set; }
        public virtual Marka? Marka { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Model może mieć maksymalnie 50 znaków.")]
        [Display(Name = "Model samochodu")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Typ paliwa")]
        public int TypPaliwaId { get; set; }
        public virtual TypPaliwa? TypPaliwa { get; set; }

        [Required]
        [RegularExpression("^[A-Z0-9]{3,10}$", ErrorMessage = "Nr rejestracyjny może zawierać tylko litery i cyfry, od 3 do 10 znaków.")]
        [Display(Name = "Numer rejestracyjny")]
        public string NrRejestracyjny { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Cena za dzień musi mieścić się w przedziale od 1 do 10 000.")]
        [Display(Name = "Cena za dzień (PLN)")]
        public decimal CenaZaDzien { get; set; }

        [Display(Name = "Dostępność")]
        public bool IsAvailable { get; set; } = true;
    }
}
