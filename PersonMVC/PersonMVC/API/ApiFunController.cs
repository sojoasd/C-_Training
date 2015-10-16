using PersonMVC.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
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

            var funDetail = db.FunDetails.ToList();
            db.FunDetails.RemoveRange(funDetail);
            db.SaveChanges();

            dynamic FunObj = new ExpandoObject();

            double betNum = 20.00;

            for (int i = 0; i < FunBasics.Count(); i++)
            {
                List<dynamic> arr = new List<dynamic>();

                FunObj.guid = FunBasics[i];
                FunObj.startDate = "2002-07-01 00:00:00";
                FunObj.firstNum = 0;
                FunObj.lastNum = 50;
                FunObj.arr = arr;
                FunObj.betNum1 = betNum - 3.99;
                FunObj.betNum2 = betNum + 3.99;
                FunObj.plusNum1 = 3.99;
                FunObj.plusNum2 = -3.99;

                insertFunData(FunObj);

                betNum += 10.00;
            }

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

            //DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            //var result = (from a in db.FunBasics
            //              join b in db.FunDetails
            //              on a.FunID equals b.FunID
            //              select new
            //              {
            //                  id = a.FunID,
            //                  name = a.FunName,
            //                  x = b.DetailYear,
            //                  y = b.DetailNav
            //              }
            //             ).GroupBy(g => new { g.id, g.name }).ToList().Select(g => new
            //             {
            //                 key = g.Key.name,
            //                 //values = (from d in g
            //                 //          orderby d.x
            //                 //          select new[] { ((TimeSpan)(d.x.Value.ToUniversalTime() - origin)).TotalSeconds, d.y }).ToArray()
            //                 //values = g.OrderBy(d => d.x).Select(d => new { x = d.x.Value, y = d.y }).ToArray()

            //                 values = g.OrderBy(d => d.x).Select(d => new[] { ((TimeSpan)(d.x.Value - origin)).TotalSeconds * 1000, d.y }).ToArray()
            //             }).AsQueryable();

            return result;
        }

        public void insertFunData(dynamic FunObj)
        {
            string startDate = FunObj.startDate;
            Guid guid = FunObj.guid;
            int firstNum = FunObj.firstNum;
            int lastNum = FunObj.lastNum;
            double betNum1 = FunObj.betNum1;
            double betNum2 = FunObj.betNum2;
            double plusNum1 = FunObj.plusNum1;
            double plusNum2 = FunObj.plusNum2;
            double tmpNum = 0.00;

            while (firstNum < lastNum)
            {
                DateTime dt = (DateTime.Parse(startDate)).AddDays(firstNum);
                //DateTime dt = (DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).AddDays(firstNum);
                FunDetail data = new FunDetail() { FunID = guid };
                data.DetailID = Guid.NewGuid();
                data.DetailYear = dt;

                Random random = new Random();
                tmpNum = (random.NextDouble() * (plusNum1 - plusNum2)) + plusNum2;

                double num = Math.Round(getRand(betNum1 + tmpNum, betNum2 + tmpNum, FunObj.arr), 2);

                if (num != 0)
                {
                    data.DetailNav = num;
                    db.FunDetails.Add(data);
                    db.SaveChanges();
                    firstNum += 1;
                }
            }
        }

        public double getRand(double minimum, double maximum, List<dynamic> arr)
        {
            Random random = new Random();

            double num = (random.NextDouble() * (maximum - minimum)) + minimum;

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