using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBConnector.Startup))]
namespace DBConnector
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
