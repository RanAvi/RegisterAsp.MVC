using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCAppRegister.Startup))]
namespace MVCAppRegister
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
