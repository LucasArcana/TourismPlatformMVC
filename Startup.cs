using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TourismPlatformMVC.Startup))]
namespace TourismPlatformMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
