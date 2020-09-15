using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebShop.Models;
using WebShop.ViewModels;
using WebShop.App_Start;
using WebShop.DAL;
using WebShop.Infrastructure;
using MailKit;

namespace WebShop.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        StoreContext db = new StoreContext();

        private IMailService mailService;
        public ManageController()
        {

        }
        public ManageController(StoreContext context, IMailService mailService)
        {
            this.mailService = mailService;
            this.db = context;
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            LinkSuccess,
            Error
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }


        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                HasPassword = this.HasPassword(),
                CurrentLogins = userLogins,
                OtherLogins = otherLogins,
                ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1,
                UserData = user.UserData
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "UserData")]UserData userData)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.UserData = userData;
                var result = await UserManager.UpdateAsync(user);

                AddErrors(result);
            }

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")]ChangePasswordViewModel model)
        {
            // In case we have simple errors - return
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            // In case we have login errors
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword([Bind(Prefix = "SetPasswordViewModel")]SetPasswordViewModel model)
        {
            // In case we have simple errors - return
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);

                if (!ModelState.IsValid)
                {
                    TempData["ViewData"] = ViewData;
                    return RedirectToAction("Index");
                }
            }

            var message = ManageMessageId.SetPasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("Index", new { Message = ManageMessageId.LinkSuccess }) : RedirectToAction("Index", new { Message = ManageMessageId.Error });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Index", new { Message = message });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }


        public ActionResult OrdersList()
        {
            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;

            IEnumerable<Zamowienie> userOrders;

            // For admin users - return all orders
            if (isAdmin)
            {
                userOrders = db.Zamowienia.Include("ZamowioneProdukty").
                    OrderByDescending(o => o.DataZamowienia).ToArray();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                userOrders = db.Zamowienia.Where(o => o.UserId == userId).Include("ZamowioneProdukty").
                    OrderByDescending(o => o.DataZamowienia).ToArray();
            }

            return View(userOrders);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public StatusZamowienia ChangeOrderState(Zamowienie order)
        {
            Zamowienie orderToModify = db.Zamowienia.Find(order.ZamowienieId);
            orderToModify.StatusZamowienia = order.StatusZamowienia;
            db.SaveChanges();

            if (orderToModify.StatusZamowienia == StatusZamowienia.Shipped)
            {
                // Schedule confirmation
                //string url = Url.Action("SendStatusEmail", "Manage", new { orderid = orderToModify.OrderId, lastname = orderToModify.LastName }, Request.Url.Scheme);

                //BackgroundJob.Enqueue(() => Helpers.CallUrl(url));

                //IMailService mailService = new HangFirePostalMailService();
                //mailService.SendOrderShippedEmail(orderToModify);

                //mailService.SendOrderShippedEmail(orderToModify);

                //dynamic email = new Postal.Email("OrderShipped");
                //email.To = orderToModify.Email;
                //email.OrderId = orderToModify.OrderId;
                //email.FullAddress = string.Format("{0} {1}, {2}, {3}", orderToModify.FirstName, orderToModify.LastName, orderToModify.Address, orderToModify.CodeAndCity);
                //email.Send();
            }

            return order.StatusZamowienia;
        }

        //[AllowAnonymous]
        //public ActionResult SendStatusEmail(int orderid, string lastname)
        //{
        //    // This could also be used (but problems when hosted on Azure Websites)
        //    // if (Request.IsLocal)            

        //    var orderToModify = db.Zamowienia.Include("ZamowioneProdukty").Include("ZamowioneProdukty.Produkt").SingleOrDefault(o => o.ZamowienieId == orderid && o.Nazwisko == lastname);

        //    if (orderToModify == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

        //    OrderShippedEmail email = new OrderShippedEmail();
        //    email.To = orderToModify.Email;
        //    email.OrderId = orderToModify.OrderId;
        //    email.FullAddress = string.Format("{0} {1}, {2}, {3}", orderToModify.FirstName, orderToModify.LastName, orderToModify.Address, orderToModify.CodeAndCity);
        //    email.Send();

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}

        //[AllowAnonymous]
        //public ActionResult SendConfirmationEmail(int orderid, string lastname)
        //{
        //    // orderid and lastname as a basic form of auth

        //    // Also might be called by scheduler (ie. Azure scheduler), pinging endpoint and using some kind of queue / db

        //    // This could also be used (but problems when hosted on Azure Websites)
        //    // if (Request.IsLocal)            

        //    var order = db.Orders.Include("ZamowioneProdukty").Include("ZamowioneProdukty.Album").SingleOrDefault(o => o.OrderId == orderid && o.LastName == lastname);

        //    if (order == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

        //    OrderConfirmationEmail email = new OrderConfirmationEmail();
        //    email.To = order.Email;
        //    email.Cost = order.TotalPrice;
        //    email.OrderNumber = order.OrderId;
        //    email.FullAddress = string.Format("{0} {1}, {2}, {3}", order.FirstName, order.LastName, order.Address, order.CodeAndCity);
        //    email.ZamowioneProdukty = order.ZamowioneProdukty;
        //    email.CoverPath = AppConfig.PhotosFolderRelative;
        //    email.Send();

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct(int? albumId, bool? confirmSuccess)
        {
            if (albumId.HasValue)
                ViewBag.EditMode = true;
            else
                ViewBag.EditMode = false;

            var result = new EditProductViewModel();
            var genres = db.Kategorie.ToArray();
            result.Kategorie = genres;
            result.ConfirmSuccess = confirmSuccess;

            Produkt a;

            if (!albumId.HasValue)
            {
                a = new Produkt();
            }
            else
            {
                a = db.Produkty.Find(albumId);
            }

            result.Produkt = a;

            return View(result);
        }

        [HttpPost]
        public ActionResult AddProduct(HttpPostedFileBase file, EditProductViewModel model)
        {
            if (model.Produkt.ProduktId > 0)
            {
                // Saving existing entry

                db.Entry(model.Produkt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddProduct", new { confirmSuccess = true });
            }
            else
            {
                // Creating new entry

                var f = Request.Form;
                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // Generate filename

                    var fileExt = Path.GetExtension(file.FileName);
                    var filename = Guid.NewGuid() + fileExt;

                    var path = Path.Combine(Server.MapPath(AppConfig.PhotosFolderRelative), filename);
                    file.SaveAs(path);

                    // Save info to DB
                    model.Produkt.NazwaObrazka = filename;
                    model.Produkt.DataDodania = DateTime.Now;

                    db.Entry(model.Produkt).State = EntityState.Added;
                    db.SaveChanges();

                    return RedirectToAction("AddProduct", new { confirmSuccess = true });
                }
                else
                {
                    ModelState.AddModelError("", "Nie wskazano pliku.");
                    var genres = db.Kategorie.ToArray();
                    model.Kategorie = genres;
                    return View(model);
                }
            }

        }

        public ActionResult HideProduct(int albumId)
        {
            var album = db.Produkty.Find(albumId);
            album.UkrytyProdukt = true;
            db.SaveChanges();

            return RedirectToAction("AddProduct", new { confirmSuccess = true });
        }

        public ActionResult UnhideProduct(int albumId)
        {
            var album = db.Produkty.Find(albumId);
            album.UkrytyProdukt = false;
            db.SaveChanges();

            return RedirectToAction("AddProduct", new { confirmSuccess = true });
        }

    }
}