using Innexys.Models;
using Innexys_Work.Models;
using Innexys_Work.Security;
using Innexys_Work.Singleton;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

//ToDo: Rewrite WebApi to be a Restful, Also rewrite routes
namespace Innexys_Work.Controllers
{
    public class TaskController : Controller
    {
        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult Create(Task newTask)
        {

            if (!SessionPersiter.Account.Tasks.Any((task) => task.Subject.Equals(newTask.Subject)))
            {

                newTask.AccountId = SessionPersiter.Account.Id;

                HttpClient clientRequest = new HttpClient();

                HttpContent reqContent = new StringContent(JsonConvert.SerializeObject(newTask),
                    Encoding.UTF8, "application/json");

                UriApiBuilder.SetUrlPath("Tasks", "CreateTask");
                
                clientRequest.PostAsync(UriApiBuilder.GetUri(), reqContent).
                    ContinueWith((createdTask) => createdTask.Result.EnsureSuccessStatusCode());

                SessionPersiter.Account.Tasks.Add(newTask);

                reqContent = new StringContent(JsonConvert.SerializeObject(SessionPersiter.Account),
                    Encoding.UTF8, "application/json");

                UriApiBuilder.SetUrlPath("Accounts", "UpdateAccount");

                clientRequest.PutAsync(UriApiBuilder.GetUri(), reqContent).
                    ContinueWith((updatedAccount) => updatedAccount.Result.EnsureSuccessStatusCode());

                UriApiBuilder.SetUrlPath("Tasks", "GetLastTaskId");

                WebRequest request = WebRequest.Create(UriApiBuilder.GetUri());
                request.ContentType = "application/json; charset=utf-8";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader dataStream = new StreamReader(response.GetResponseStream()))
                    {
                        SessionPersiter.Account.Tasks.Last().Id = JsonConvert.DeserializeObject<int>(dataStream.ReadToEnd()) + 1;
                        SessionPersiter.Account.Tasks.Last().Account = SessionPersiter.Account;
                    }
                }

                return RedirectToAction("Innexys", "Account");
            }
            else
            {
                ViewBag.Error = "This task already exists";
                return RedirectToAction("Create");
            }
        }

        [CustomAuthorize]
        public ActionResult ViewPersonalTasks()
        {
            return View(SessionPersiter.Account.Tasks);
        }

        [CustomAuthorize]
        public ActionResult EditTask(string subject)
        {
            //ToDo Call WebApi
            return View(new TaskModel().Find(subject));
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult EditTask(Task task)
        {
            Task exisitngtask = SessionPersiter.Account.Tasks.FirstOrDefault((exisitngTask) => exisitngTask.Id == task.Id);
            exisitngtask.Subject = task.Subject;    
            exisitngtask.StartDate = task.StartDate;
            exisitngtask.EndDate = task.EndDate;
            exisitngtask.Description = task.Description;
            exisitngtask.Category = task.Category;

            HttpClient clientRequest = new HttpClient();

            UriApiBuilder.SetUrlPath("Tasks", "UpdateTask");

            HttpContent reqContent = new StringContent(JsonConvert.SerializeObject(exisitngtask),
                Encoding.UTF8, "application/json");

            clientRequest.PutAsync(UriApiBuilder.GetUri(), reqContent).
                    ContinueWith((updateTask) => updateTask.Result.EnsureSuccessStatusCode());

            return RedirectToAction("Innexys","Account");
        }
    }
}