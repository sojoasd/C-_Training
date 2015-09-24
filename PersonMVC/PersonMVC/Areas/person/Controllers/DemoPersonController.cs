using Newtonsoft.Json.Linq;
using personMVC.Controllers;
using PersonMVC.Controllers;
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
using System.Web.Script.Serialization;

namespace personMVC.Areas.person.Controllers
{
    public class DemoPersonController : CustomController
    {
        private AdventureWorksLT2012Entities adb = new AdventureWorksLT2012Entities();

        public ActionResult Index()
        {
            var person_data = adb.DemoPersons;
            //ViewBag.showList = person_data.ToList();
            //ViewData["person_data"] = person_data.ToList();

            //var infoFabAreas = db.InfoFabAreas.Include(i => i.InfoFabUser).Include(i => i.InfoFabUser1);

            int takenum = 0;
            int skipnum = 0;

            CreatePage(person_data.Count(), null, out skipnum, out takenum);
            var data = person_data.OrderBy(c => c.PersonID).Skip(skipnum).Take(takenum).ToList();
            ViewBag.showList = data;
            return View();
        }

        //public ActionResult isLogin()
        //{
        //    return RedirectToAction("Index");
        //}

        //public ActionResult Logout()
        //{
        //    Session["user"] = false;

        //    return RedirectToAction("Index");
        //}

        public ActionResult OtherPage(string ActionName, Guid? id)
        {
            ViewBag.isCreate = false;
            ViewBag.isEdit = false;
            ViewBag.isDelete = false;

            var person_data = adb.DemoPersons;

            switch (ActionName)
            {
                case "isCreate":
                    ViewBag.isCreate = true;
                    ViewBag.data = person_data.ToList();
                    break;

                case "isEdit":
                    ViewBag.isEdit = true;
                    if (id != null)
                    {
                        ViewBag.data = person_data.Where(c => c.PersonID == id).ToList();
                    }
                    break;

                case "isDelete":
                    ViewBag.isDelete = true;
                    break;
            }

            string area = Request.RequestContext.RouteData.DataTokens.ContainsKey("area") ? Request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
            string controller = Request.RequestContext.RouteData.Values["controller"].ToString();

            return PartialView("~/Areas/" + area + "/Views/" + controller + "/OtherPage.cshtml");
        }

        public ActionResult Create()
        {
            ViewBag.isCreate = true;
            return View();
        }

        [HttpPost]
        public ActionResult Create(DemoPerson person)
        {
            if (ModelState.IsValid)
            {
                person.PersonID = Guid.NewGuid();
                adb.DemoPersons.Add(person);
                adb.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DemoPersonViewModel personViewModel = getObject(id.HasValue ? (Guid)id : Guid.Empty);

            if (personViewModel == null)
            {
                return HttpNotFound();
            }

            return View(personViewModel);
        }

        [HttpPost]
        public ActionResult Edit(DemoPersonViewModel personViewModel)
        {
            if (ModelState.IsValid)
            {
                DemoPerson person = adb.DemoPersons.Find(personViewModel.PersonID);
                person.PersonSex = personViewModel.BoolSex ? 1 : 0;
                person.PersonID = personViewModel.PersonID;
                person.PersonName = personViewModel.PersonName;
                person.PersonBirthday = personViewModel.PersonBirthday;

                adb.Entry(person).State = EntityState.Modified;
                adb.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit");
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DemoPersonViewModel personViewModel = getObject(id.HasValue ? (Guid)id : Guid.Empty);

            if (personViewModel == null)
            {
                return HttpNotFound();
            }

            return View(personViewModel);
        }

        public DemoPersonViewModel getObject(Guid id)
        {
            DemoPerson person = adb.DemoPersons.Find(id);
            DemoPersonViewModel personViewModel = new DemoPersonViewModel();

            personViewModel.BoolSex = person.PersonSex == 1 ? true : false;
            personViewModel.PersonID = person.PersonID;
            personViewModel.PersonName = person.PersonName;
            personViewModel.PersonBirthday = person.PersonBirthday;

            return personViewModel;
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            DemoPerson person = adb.DemoPersons.Find(id);
            adb.DemoPersons.Remove(person);
            adb.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AjaxTest(string jsonData)
        {
            //JavaScriptSerializer jobj = new JavaScriptSerializer();
            //object json = jobj.Deserialize(jsonData, typeof(object));
            dynamic json = JObject.Parse(jsonData);
            string personName = json.personName;

            int count = (
                    from emp in adb.DemoPersons
                    where emp.PersonName == personName
                    select emp
                 ).Count();

            if (count > 0)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }
    }
}