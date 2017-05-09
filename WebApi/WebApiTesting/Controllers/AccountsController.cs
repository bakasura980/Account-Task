using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTesting.DataConnection;
using WebApiTesting.Models;

namespace WebApiTesting.Controllers
{
    public class AccountsController : ApiController
    {
        IRepository<Account> accounts = new GenericRepository<Account>();


        // GET api/<controller>
        public IEnumerable<Account> GetAccounts()
        {
            return accounts.All();
        }
        public IHttpActionResult GetAccount(string email)
        {
            Account currentAccount = accounts.GetExactly((acc) => acc.Email.Equals(email));

            if (currentAccount == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(currentAccount);
            }
        }

        [HttpGet]
        public IHttpActionResult GetLastRegisteredAccount()
        {
            return Ok(accounts.All().Last());
        }

        // POST api/<controller>
        [HttpPost]
        public void CreateAccount([FromBody]Account newAccount)
        {
            accounts.Add(newAccount);
            accounts.SaveChanges();
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void UpdateAccount([FromBody]Account editedAccount)
        {
            accounts.Update(editedAccount);
            accounts.SaveChanges();
        }
    }
}