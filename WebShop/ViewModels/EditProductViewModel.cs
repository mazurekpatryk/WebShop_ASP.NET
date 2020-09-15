using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Models;

namespace WebShop.ViewModels
{
    public class EditProductViewModel
    {
        public Produkt Produkt { get; set; }
        public IEnumerable<Kategoria> Kategorie { get; set; }
        public bool? ConfirmSuccess { get; set; }

    }
}