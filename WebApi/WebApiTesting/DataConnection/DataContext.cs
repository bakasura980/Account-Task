using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApiTesting.Models;

namespace WebApiTesting.DataConnection
{
    public class DataContext : DbContext
    {
        public DataContext()
           : base("Innexys")
        {
            Configuration.ProxyCreationEnabled = true;
        }

        public IDbSet<Account> Accounts { get; set; }

        public IDbSet<Task> Tasks { get; set; }

    }
}
