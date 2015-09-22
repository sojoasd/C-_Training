using Newtonsoft.Json;
using PersonMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        /// 2. json post : 要選擇 raw 再選 json，並將 Content-Type : application/json
        /// ps. 這兩者都不能放入 PersonID 欄位，否則會 binding 沒過
        /// person = collection data
        /// </summary>
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

        /// <summary>
        /// post man 作法同上面
        /// </summary>
        [ResponseType(typeof(DemoPerson))]
        public IHttpActionResult PutCustomer(Guid? id, DemoPerson person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.PersonID)
            {
                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(person);
        }

        [ResponseType(typeof(DemoPerson))]
        public IHttpActionResult DeleteProduct(Guid? id)
        {
            DemoPerson person = db.DemoPersons.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.DemoPersons.Remove(person);
            db.SaveChanges();

            return Ok(person);
        }
    }
}