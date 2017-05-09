using Innexys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Innexys_Work.Security
{
    public class SessionPersiter
    {
        static string accountSessionvar = "account";

        public static Account Account
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }
                var sessionvar = HttpContext.Current.Session[accountSessionvar];
                if (sessionvar != null)
                {
                    return sessionvar as Account;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[accountSessionvar] = value;
            }
        }
    }
}