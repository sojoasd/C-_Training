using System.Web.Mvc;

namespace PersonMVC.Areas.nvd3
{
    public class nvd3AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "nvd3";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "nvd3_default",
                "nvd3/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}