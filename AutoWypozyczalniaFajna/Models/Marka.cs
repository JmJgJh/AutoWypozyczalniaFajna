using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoWypozyczalniaFajna.Models
{
    public class Marka
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Marka może mieć maksymalnie 50 znaków.")]
        [DisplayName("Marka")]
        public string MarkaNazwa { get; set; }
    }
}
