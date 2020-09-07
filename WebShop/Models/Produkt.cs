using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class Produkt
    {
        public int ProduktId { get; set; }
        public int KategoriaId { get; set; }
        public string Nazwa { get; set; }
        public string Opis{ get; set; }
        public DateTime DataDodania { get; set; }
        public decimal Cena { get; set; }
        public bool UkrytyProdukt { get; set; }
        public string NazwaObrazka { get; set; }
        public virtual Kategoria Kategoria { get; set; }

    }
}