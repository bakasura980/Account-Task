using Innexys_Work.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Innexys_Work.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersiter.Email))
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Account", index = "Login" }));
            else
            {
                AccountModel accmodel = new AccountModel();
                CustomPrincipal customprincipal = new CustomPrincipal(accmodel.Find(SessionPersiter.Email));
                if (!customprincipal.IsInRole(Roles))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Login" }));
                }
            }
                    
        }
    }
}