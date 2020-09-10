using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebGrease.Css.Ast;
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

        public ActionResult List(string type, string searchQuery = null)
        {
            var kategoria = db.Kategorie.Include("Produkts").Where(g => g.Name.ToUpper() == type.ToUpper()).Single();
            var produkty = kategoria.Produkts.Where(a => (searchQuery == null ||
                                            a.Nazwa.ToLower().Contains(searchQuery.ToLower())) &&
                                            !a.UkrytyProdukt);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", produkty);
            }

            return View(produkty);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 80000)]
        public ActionResult GMenuKategori()
        {
            var kategorie = db.Kategorie.ToList();

            return PartialView("_GMenuKategori",kategorie); 
        }

        public ActionResult ProductsSuggestions(string term)
        {
            var products = this.db.Produkty.Where(a => !a.UkrytyProdukt && a.Nazwa.ToLower().Contains(term.ToLower())).Take(5).Select(a => new { label = a.Nazwa });
            return Json(products, JsonRequestBehavior.AllowGet);

        }
    }
}