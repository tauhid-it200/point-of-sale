using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PosMvc.Startup))]
namespace PosMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
