using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UmbracoMVCBundling.Startup))]
namespace UmbracoMVCBundling
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
