using Newtonsoft.Json;
using PersonMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace PersonMVC.API
{
    public class ApiDataController : ApiController
    {
        private AdventureWorksLT2012Entities db = new AdventureWorksLT2012Entities();

        /// <summary>
        /// 最原始的 GET 回傳，沒有傳入任何引數
        /// </summary>
        /// <returns></returns>
        public IQueryable GetDemoPerson()
        {
            return db.DemoPersons;
        }

        [ResponseType(typeof(DemoPerson))]
        public IHttpActionResult GetDemoPerson(Guid? id)
        {
            DemoPerson person = db.DemoPersons.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        /// <summary>
        /// 這裡的 postman
        /// 1. form post : 要選擇 x-www-form-urlencoded
        /// 2. json post : 要選擇 raw -> json
        /// ps. 這兩者都不能放入 PersonID 欄位，否則會 binding 沒過
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [ResponseType(typeof(DemoPerson))]
        public IHttpActionResult PostCustomer(DemoPerson person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DemoPerson DPDdata = new DemoPerson() { PersonID = Guid.NewGuid() };

            DPDdata.PersonName = person.PersonName;
            DPDdata.PersonSex = person.PersonSex;
            DPDdata.PersonBirthday = person.PersonBirthday;

            db.DemoPersons.Add(DPDdata);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = DPDdata.PersonID }, DPDdata);
        }
    }
}