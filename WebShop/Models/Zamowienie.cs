using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebShop.DAL;

namespace WebShop.Models
{
    public class Zamowienie
    {
        public int ZamowienieId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required(ErrorMessage = "Musisz wprowadzić imię")]
        [StringLength(100)] 
        public string Imie { get; set; }
        [Required(ErrorMessage = "Musisz wprowadzć nazwisko")]
        [StringLength(150)] 
        public string Nazwisko { get; set; }
        [Required(ErrorMessage = "Nie wprowadzono adresu")]
        [StringLength(150)] 
        public string Adres { get; set; }
        [Required(ErrorMessage = "Wprowadź kod pocztowy i miasto")]
        [StringLength(50)] 
        public string KodPocztowy { get; set; }
        [Required(ErrorMessage = "Musisz wprowadzić numer telefonu")]
        [StringLength(20)]
        [RegularExpression(@"(\+\d{2})*[\d\s-]+",
            ErrorMessage = "Błędny format numeru telefonu.")]
        public string NumerTelefonu { get; set; }
        [Required(ErrorMessage = "Wprowadź swój adres e-mail.")]
        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }
        public string Komentarz { get; set; }
        public DateTime DataZamowienia { get; set; }
        public StatusZamowienia StatusZamowienia { get; set; }
        public decimal CenaZbiorcza { get; set; }
        public List<ZamowionyProdukt> ZamowioneProdukty { get; set; }
    }

    public enum StatusZamowienia
    {
        [Display(Name = "nowe")] 
        New,

        [Display(Name = "wysłane")] 
        Shipped
    }
}