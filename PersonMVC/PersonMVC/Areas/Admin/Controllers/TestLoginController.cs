using Newtonsoft.Json.Linq;
using PersonMVC.Filter;
using PersonMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PersonMVC.Areas.Admin.Controllers
{
    public class TestLoginController : Controller
    {
        private AdventureWorksLT2012Entities db = new AdventureWorksLT2012Entities();

        // GET: Admin/TestLogin
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Login()
        //{
        //    Session["user"] = false;
        //    string name = Request["Name"].ToString();
        //    int num = db.DemoPersons.Where(w => w.PersonName == name).Count();

        //    if (num == 0)
        //    {
        //        Session["user"] = false;
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        Session["user"] = true;
        //        return RedirectToAction("Index", "DemoPerson", new { area = "person" });
        //    }
        //}

        [HttpPost]
        public ActionResult Login()
        {
            string name = Request["Name"].ToString();
            int num = db.DemoPersons.Where(w => w.PersonName == name).Count();

            if (num == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                HttpCookie cookie = new HttpCookie("user");
                cookie.Value = name + DateTime.Now.ToShortTimeString();
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "DemoPerson", new { area = "person" });
            }
        }

        public ActionResult Logout()
        {
            bool isCookie = this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("user");

            if (isCookie)
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["user"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index");
        }
    }
}