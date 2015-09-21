using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PersonMVC.Startup))]
namespace PersonMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
