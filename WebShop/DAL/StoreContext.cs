using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebShop.Models;

namespace WebShop.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base("StoreContext")
        {

        }
        public DbSet<Kategoria>Kategorie { get; set; }
        public DbSet<Produkt>Produkty { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<ZamowionyProdukt> ZamowioneProdukty { get; set; }
    }
}