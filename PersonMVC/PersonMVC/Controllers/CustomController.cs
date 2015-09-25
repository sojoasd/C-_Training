using PersonMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PersonMVC.Controllers
{
    public class CustomController : Controller
    {
        public void CreatePage(int dataCount, int page, out int skipnum, out int takenum)
        {
            int total = dataCount;
            TempData["totalcount"] = total;

            //每頁幾筆
            int per_num = 10;
            TempData["per_num"] = per_num;

            //接收頁數，並建立目前是第幾頁
            int page_num = (page == 0) ? 1 : page;
            skipnum = (page_num == 0) ? 0 : (page_num - 1) * per_num;
            takenum = per_num;
            TempData["now_page"] = (page_num == 0) ? 1 : page_num;
        }

        public ActionResult returnJS()
        {
            List<PaginationData> data = new List<PaginationData>()
            {
                new PaginationData(){
                    totalcount = Convert.ToInt32(TempData["totalcount"]),
                    per_num = Convert.ToInt32(TempData["per_num"]),
                    now_page = Convert.ToInt32(TempData["now_page"])
                }
            };
            //string json_data = JsonConvert.SerializeObject(data);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

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