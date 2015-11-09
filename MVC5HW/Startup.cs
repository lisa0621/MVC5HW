using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5HW.Startup))]
namespace MVC5HW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
