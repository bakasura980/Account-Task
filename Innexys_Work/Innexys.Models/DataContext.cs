using Innexys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Innexys.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("Innexys")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public IDbSet<Account> Accounts { get; set; }

        public IDbSet<Innexys.Models.Task> Tasks { get; set; }
    }
}
