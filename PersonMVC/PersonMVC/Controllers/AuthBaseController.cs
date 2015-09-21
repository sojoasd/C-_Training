using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace personMVC.Controllers
{
    public class AuthBaseController : Controller
    {
        // GET: AuthBase
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    bool isSession = Convert.ToBoolean(filterContext.HttpContext.Session["user"]);

        //    if (!isSession)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
        //                new { action = "Index", controller = "TestLogin", area = "Admin" }));
        //    }
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isCookie = Convert.ToBoolean(filterContext.HttpContext.Request.Cookies.AllKeys.Contains("user"));

            if (!isCookie)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new { action = "Index", controller = "TestLogin", area = "Admin" }));
            }
        }
    }
}