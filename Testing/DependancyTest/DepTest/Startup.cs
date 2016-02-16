using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DepTest.Startup))]
namespace DepTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
