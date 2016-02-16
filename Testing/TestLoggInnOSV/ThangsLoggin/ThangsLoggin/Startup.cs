using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThangsLoggin.Startup))]
namespace ThangsLoggin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
