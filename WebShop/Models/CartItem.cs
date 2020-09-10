using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class CartItem
    {
        public Produkt Produkt { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}