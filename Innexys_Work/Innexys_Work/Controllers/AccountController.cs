using Innexys.Models;
using Innexys_Work.Models;
using Innexys_Work.Security;
using Innexys_Work.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Innexys_Work.Singleton;

namespace Innexys_Work.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            UriApiBuilder.SetUrlPathWithParam("Accounts", "GetAccount", "email", $"{account.Email}");
            WebRequest request = WebRequest.Create(UriApiBuilder.GetUri());

            request.ContentType = "application/json; charset=utf-8";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader dataStream = new StreamReader(response.GetResponseStream()))
                    {
                        Account gottenAccount = JsonConvert.DeserializeObject<Account>(dataStream.ReadToEnd());
                        if (gottenAccount.Name.Equals(account.Name))
                        {
                            SessionPersiter.Account = gottenAccount;
                            return RedirectToAction("Innexys");
                        }
                        else
                        {
                            ViewBag.Error = "The Name does not match this email";
                            return View("Login");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "This Account does not exists";
                return View("Login");
            }
        }
        
        public ActionResult Logout()
        {
            SessionPersiter.Account = null;
            return RedirectToAction("Login");
        }

        [CustomAuthorize]
        public ActionResult Innexys()
        {
            UriApiBuilder.SetUrlPath("Accounts","GetAccounts");

            WebRequest request = WebRequest.Create(UriApiBuilder.GetUri());
            request.ContentType = "application/json; charset=utf-8";

            ICollection<Account> accounts;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader dataStream = new StreamReader(response.GetResponseStream()))
                {
                    accounts = JsonConvert.DeserializeObject <ICollection<Account>>(dataStream.ReadToEnd());

                }
            }
            return View(accounts);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            UriApiBuilder.SetUrlPathWithParam("Accounts", "GetAccount", "email", $"{account.Email}");

            WebRequest request = WebRequest.Create(UriApiBuilder.GetUri());

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    ViewBag.Error = "This Account exists, try another email";
                    return View("Register");
                }
            }
            catch (Exception)
            {
                HttpClient clientPostRequest = new HttpClient();
                HttpContent postContent = new StringContent(JsonConvert.SerializeObject(account), 
                    Encoding.UTF8, "application/json");

                UriApiBuilder.SetUrlPath("Accounts","CreateAccount");

                clientPostRequest.PostAsync(UriApiBuilder.GetUri(), postContent).
                    ContinueWith((createdAccount) => createdAccount.Result.EnsureSuccessStatusCode());

                UriApiBuilder.SetUrlPath("Accounts", "GetLastRegisteredAccount");
                request = WebRequest.Create(UriApiBuilder.GetUri());
                request.ContentType = "application/json; charset=utf-8";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader dataStream = new StreamReader(response.GetResponseStream()))
                    {
                        SessionPersiter.Account = JsonConvert.DeserializeObject<Account>(dataStream.ReadToEnd());
                    }
                }

                return RedirectToAction("Innexys");
            }
        }

        [CustomAuthorize]
        public ActionResult Edit()
        {
            return View(SessionPersiter.Account);
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult Edit(Account account)
        {
            HttpClient clientPutRequest = new HttpClient();
            HttpContent putContent = new StringContent(JsonConvert.SerializeObject(account),
                Encoding.UTF8, "application/json");

            UriApiBuilder.SetUrlPath("Accounts","UpdateAccount");

            clientPutRequest.PutAsync(UriApiBuilder.GetUri(), putContent).
                ContinueWith((updatedAccount) => updatedAccount.Result.EnsureSuccessStatusCode());

            SessionPersiter.Account.Name = account.Name;
            SessionPersiter.Account.Email = account.Email;
            SessionPersiter.Account.Phone = account.Phone;
            SessionPersiter.Account.Position = account.Position;

            return RedirectToAction("Innexys","Account");
        }
    }
}