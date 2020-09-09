using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.DAL;

namespace WebShop.Controllers
{
    public class StoreController : Controller
    {
        StoreContext db = new StoreContext(); 

        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var produkt = db.Produkty.Find(id);

            return View(produkt);
        }

        public ActionResult List(string type)
        {
            var kategoria = db.Kategorie.Include("Produkts").Where(g => g.Name.ToUpper() == type.ToUpper()).Single();
            var produkty = kategoria.Produkts.ToList();

            return View(produkty);
        }
        [ChildActionOnly]

        public ActionResult GMenuKategori()
        {
            var kategorie = db.Kategorie.ToList();

            return PartialView("_GMenuKategori",kategorie); 
        }
    }
}