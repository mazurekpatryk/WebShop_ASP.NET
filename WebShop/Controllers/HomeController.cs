using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.DAL;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Home
        public ActionResult Index()
        {
            Kategoria newKategoria = new Kategoria { Name = "Skarpetki" };
            db.Kategorie.Add(newKategoria);
            db.SaveChanges();

            return View();
        }

        public ActionResult StaticContent(string viewname)
        {
            return View(viewname);
        }
    }
}