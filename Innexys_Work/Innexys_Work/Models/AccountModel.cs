using Innexys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Innexys_Work.Models
{
    public class AccountModel
    {
        IRepository<Account> accounts;

        public AccountModel(IRepository<Account> accounts)
        {
            this.accounts = accounts;
        }

        public AccountModel()
        {
            this.accounts = new GenericRepository<Account>();
        }

        public Account Find(string email)
        {
            return accounts.All().Where(acc => acc.Email.Equals(email)).FirstOrDefault();
        }

        public Account Login(string username, string email)
        {
            return accounts.All().Where(acc => acc.Name.Equals(username) && acc.Email.Equals(email)).FirstOrDefault();
        }

        public bool Register(Account account)
        {
            if (!accounts.All().Any(acc => acc.Email.Equals(account.Email)))
            {
                accounts.Add(account);
                accounts.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(Account account)
        {
            accounts.Update(account);
            accounts.SaveChanges();
        }

        public IQueryable<Account> All()
        {
            return accounts.All();
        }
    }
}