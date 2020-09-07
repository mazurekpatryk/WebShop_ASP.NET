using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class ZamowionyProdukt
    {
        public int ZamowionyProduktId { get; set; }
        public int ZamowieniaId { get; set; }
        public int ProduktId { get; set; }
        public int Ilosc { get; set; }
        public decimal CenaJednostkowa { get; set; }
        public virtual Produkt Produkt { get;set;}
        public virtual Zamowienie Zamowienia{ get; set; }

    }
}