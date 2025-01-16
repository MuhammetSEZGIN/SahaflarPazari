using Microsoft.Owin;
using Owin;
using Infrastructure.Identity;

[assembly: OwinStartup(typeof(SahaflarPazari.App_Start.Startup))]

namespace SahaflarPazari.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var identityConfig = new IdentityConfig();
            identityConfig.Configuration(app);
        }
    }
}