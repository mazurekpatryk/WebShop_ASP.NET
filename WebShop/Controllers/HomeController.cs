using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.DAL;
using WebShop.Models;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Home
        public ActionResult Index()
        {
            var kategorie = db.Kategorie.ToList();

            var najnowszeProdukty = db.Produkty.Where(a => !a.UkrytyProdukt).OrderByDescending(a => a.DataDodania).ToList();

            var vm = new HomeViewModel()
            {
                Kategorie = kategorie,
                NajnowszeProdukty = najnowszeProdukty
            };



            return View(vm);
        }

        public ActionResult StaticContent(string viewname)
        {
            return View(viewname);
        }
    }
}