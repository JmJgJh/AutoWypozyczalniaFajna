using System.ComponentModel.DataAnnotations;

namespace AutoWypozyczalniaFajna.Models
{
    public class TypPaliwa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Typ paliwa może mieć maksymalnie 50 znaków.")]
        public string Paliwo { get; set; }
    }
}
