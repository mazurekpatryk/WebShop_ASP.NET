using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl= returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModels model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
                return RedirectToAction("Index", "Home");
        }

    }
}