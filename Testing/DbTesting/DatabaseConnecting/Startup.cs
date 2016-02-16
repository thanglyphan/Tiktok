using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DatabaseConnecting.Startup))]
namespace DatabaseConnecting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
