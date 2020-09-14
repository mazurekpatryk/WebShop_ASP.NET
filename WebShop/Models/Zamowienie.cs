using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.DAL;

namespace WebShop.Models
{
    public class Zamowienie
    {
        public int ZamowienieId { get; set; }
        public string UzytkownikId { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Adres { get; set; }
        public string KodPocztowy { get; set; }
        public string NumerTelefonu { get; set; }  
        public string Email { get; set; }
        public string Komentarz { get; set; }
        public DateTime DataZamowienia { get; set; }
        public StatusZamowienia StatusZamowienia { get; set; }
        public decimal CenaZbiorcza { get; set; }
        public List<ZamowionyProdukt> ZamowioneProdukty { get; set; }
    }

    public enum StatusZamowienia
    {
        New,

        Shipped
    }
}