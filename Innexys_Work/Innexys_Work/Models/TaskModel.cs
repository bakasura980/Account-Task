using Innexys.Models;
using Innexys_Work.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Innexys_Work.Models
{
    public class TaskModel
    {
        IRepository<Task> tasks;
        IRepository<Account> accounts;

        public TaskModel(IRepository<Task> tasks, IRepository<Account> accounts)
        {
            this.tasks = tasks;
            this.accounts = accounts;
        }

        public TaskModel()
        {
            this.tasks = new GenericRepository<Task>();
            this.accounts = new GenericRepository<Account>();
        }        

        public Task Find(string subject)
        {
            return tasks.All().Where(tasksubject => tasksubject.Subject.Equals(subject)).FirstOrDefault();
        }

        public bool CreateTask(Task task, string customeremail)
        {
            if (Find(task.Subject) == null)
            {
                AccountModel accmodel = new AccountModel();
                Account account = accmodel.Find(customeremail);
                task.AccountId = account.Id;
                tasks.Add(task);
                tasks.SaveChanges();
                account.Tasks.Add(task);
                accounts.Update(account);
                accounts.SaveChanges();
                return true;
            }else
            {
                return false;
            }
        }

        public IQueryable<Task> ShowTasks(string email)
        {
            return accounts.All().Where(customer => customer.Email.Equals(email)).SelectMany(tasks => tasks.Tasks);
        }

        public void Update(Task task, string customeremail)
        {
            task.AccountId = new AccountModel().Find(customeremail).Id;
            tasks.Update(task);
            tasks.SaveChanges();
        }
    }
}