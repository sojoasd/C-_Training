using PersonMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PersonMVC.API
{
    [EnableCors(origins: "http://localhost:5705", headers: "*", methods: "*")]
    public class ApiFunController : ApiController
    {
        private FunDataEntities db = new FunDataEntities();

        public IQueryable Get()
        {
            var FunBasics = db.FunBasics.Select(f => f.FunID).ToList();

            //Guid guid = FunBasics[2];

            //List<dynamic> arr = new List<dynamic>();

            //int n = 0;

            //while (n < 100)
            //{
            //    DateTime dt = (DateTime.Parse("2000-01-01")).AddDays(n);
            //    FunDetail data = new FunDetail() { FunID = guid };
            //    data.DetailID = Guid.NewGuid();
            //    data.DetailYear = dt;

            //    double num = Math.Round(getRand(40.00, 79.99, arr), 2);

            //    if (num != 0)
            //    {
            //        data.DetailNav = num;
            //        db.FunDetails.Add(data);
            //        db.SaveChanges();
            //        n += 1;
            //    }
            //}

            var result = (from a in db.FunBasics
                          join b in db.FunDetails
                          on a.FunID equals b.FunID
                          select new
                          {
                              id = a.FunID,
                              name = a.FunName,
                              x = b.DetailYear,
                              y = b.DetailNav
                          }
                         ).GroupBy(g => new { g.id, g.name }).ToList().Select(g => new
                            {
                                id = g.Key.id,
                                name = g.Key.name,
                                data = g.Select(d => new { x = d.x.Value.ToString("yyyy-MM-dd"), y = d.y }).ToList().OrderBy(c => c.x)
                            }).AsQueryable();

            return result;
        }

        public float getRand(double minimum, double maximum, List<dynamic> arr)
        {
            Random random = new Random();

            double num = 20 + ((random.NextDouble() * (maximum - minimum)) + minimum) / 11.11;

            if (arr.Contains(num))
            {
                num = 0;
            }
            else
            {
                arr.Add(num);
            }

            return Convert.ToSingle(num);
        }
    }
}