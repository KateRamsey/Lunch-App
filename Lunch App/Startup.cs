using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lunch_App.Startup))]
namespace Lunch_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
