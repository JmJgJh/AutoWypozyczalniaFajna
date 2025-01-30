using AutoWypozyczalniaFajna.Validators;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using AutoWypozyczalniaFajna.Data;

namespace AutoWypozyczalniaFajna.Models
{
    public class Wypozyczenie
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Samochod")]
        public int SamochodId { get; set; }
        public virtual Samochod? Samochod { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data wypożyczenia")]
        public DateTime DataWypozyczenia { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data zwrotu")]
        [GreaterThan("DataWypozyczenia", ErrorMessage = "Data zwrotu musi być późniejsza niż data wypożyczenia.")]
        public DateTime DataZwrotu { get; set; } = DateTime.Today;

        [Display(Name = "Cena Całkowita")]
        public decimal CenaCalkowita { get; set; }
        [Display(Name = "Użytkownik")]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }
    }
}
