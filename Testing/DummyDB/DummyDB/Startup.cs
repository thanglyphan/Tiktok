using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DummyDB.Startup))]
namespace DummyDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
