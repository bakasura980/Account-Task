using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Innexys_Work.Security
{
    public class SessionPersiter
    {
        static string usernameSessionvar = "email";

        public static string Email
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                var sessionvar = HttpContext.Current.Session[usernameSessionvar];
                if (sessionvar != null)
                {
                    return sessionvar as string;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[usernameSessionvar] = value;
            }
        }
    }
}