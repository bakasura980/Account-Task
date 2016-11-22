using Innexys.Models;
using Innexys_Work.Models;
using Innexys_Work.Security;
using Innexys_Work.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Innexys_Work.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel accviewmodel)
        {
            AccountModel accmodel = new AccountModel();
            if (accmodel.Login(accviewmodel.Account.Name, accviewmodel.Account.Email) == null)
            {
                ViewBag.Error = "This Account does not exists";
                return View("Login");
            }
            SessionPersiter.Email = accviewmodel.Account.Email;
            return RedirectToAction("Innexys");
        }

        [CustomAuthorize]
        public ActionResult Innexys()
        {
            AccountModel accmodel = new AccountModel();
            return View(accmodel.All());
        }

        public ActionResult Logout()
        {
            SessionPersiter.Email = string.Empty;
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            AccountModel accmodel = new AccountModel();
            if (accmodel.Register(account))
            {
                SessionPersiter.Email = account.Email;
                return RedirectToAction("Innexys");
            }
            else
            {
                return RedirectToAction("Register"); 
            }
        }

        private ActionResult ReturnFind()
        {
            AccountModel accmodel = new AccountModel();
            return View(accmodel.Find(SessionPersiter.Email));
        }

        [CustomAuthorize]
        public ActionResult ViewAccount()
        {
            return ReturnFind();
        }

        [CustomAuthorize]
        public ActionResult Edit()
        {
            return ReturnFind();
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult Edit(Account account)
        {
            AccountModel accmodel = new AccountModel();
            accmodel.Update(account);
            return RedirectToAction("Innexys");
        }
    }
}