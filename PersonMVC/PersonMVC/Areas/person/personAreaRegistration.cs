using System.Data;
using System.Data.Entity;
using System.Web.Mvc;

namespace personMVC.Areas.person
{
    public class personAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "person";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "person_Test",
                "person/{controller}/{action}/{page}",
                defaults: new { action = "DemoPerson", id = UrlParameter.Optional, page = "0" },
                constraints: new { action = "Index" } // 當 Action = Index 時才會執行這個 route
            );

            context.MapRoute(
                "person_default",
                "person/{controller}/{action}/{id}/{page}",
                new { action = "DemoPerson", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );
        }
    }
}