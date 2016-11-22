using Innexys.Models;
using Innexys_Work.Models;
using Innexys_Work.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Create(Task task)
        {
            TaskModel taskmodel = new TaskModel();
            if (taskmodel.CreateTask(task, SessionPersiter.Email))
            {
                return RedirectToAction("Innexys", "Account");
            }else
            {
                ViewBag.Error = "This task already exists";
                return RedirectToAction("Create");
            }
            
        }

        [CustomAuthorize]
        public ActionResult ViewTasks()
        {
            return View(new TaskModel().ShowTasks(SessionPersiter.Email));
        }

        [CustomAuthorize]
        public ActionResult EditTask(string subject)
        {
            return View(new TaskModel().Find(subject));
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult EditTask(Task task)
        {
            TaskModel taskmodel = new TaskModel();
            taskmodel.Update(task, SessionPersiter.Email);
            return RedirectToAction("Innexys","Account");
        }
    }
}