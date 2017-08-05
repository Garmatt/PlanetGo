using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CloudGoClub.Startup))]
namespace CloudGoClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
