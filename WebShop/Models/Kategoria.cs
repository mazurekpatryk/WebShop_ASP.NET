using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class Kategoria
    {
        public int KategoriaId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Produkt> Produkts { get; set; }
    }
}