using Innexys.Models;
using Innexys_Work.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Innexys_Work.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionPersiter.Account == null)
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Login" }));
            else
            {
                CustomPrincipal customprincipal = new CustomPrincipal(SessionPersiter.Account);
                if (!customprincipal.IsInRole(Roles))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Login" }));
                }
            }
        }

    }
}