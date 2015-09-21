using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace PersonMVC.Filter
{
    public class TestAuth : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //filterContext.Principal = new ClaimsPrincipal();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            bool isSession = Convert.ToBoolean(filterContext.HttpContext.Session["user"]);

            if (!isSession)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new { action = "Index", controller = "TestLogin", area = "Admin" }));
            }
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    bool isSession = Convert.ToBoolean(filterContext.HttpContext.Session["user"]);

        //    if (isSession)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
        //                new { action = "Index", controller = "DemoPerson", area = "person" }));
        //    }
        //    else
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
        //                new { action = "Index", controller = "TestLogin", area = "Admin" }));
        //    }
        //}
    }
}