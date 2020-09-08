using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Models;

namespace WebShop.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Kategoria> Kategorie { set; get; }
    }
}