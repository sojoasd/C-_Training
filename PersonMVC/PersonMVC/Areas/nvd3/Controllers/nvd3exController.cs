using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonMVC.Areas.nvd3.Controllers
{
    public class nvd3exController : Controller
    {
        // GET: nvd3/nvd3ex
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CumulativeLineChart()
        {
            return View();
        }

        public ActionResult LineChart()
        {
            return View();
        }
    }
}