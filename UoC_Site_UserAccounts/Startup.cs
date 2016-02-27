using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UoC_Site_UserAccounts.Startup))]
namespace UoC_Site_UserAccounts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
