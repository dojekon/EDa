using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EDa.Startup))]
namespace EDa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
