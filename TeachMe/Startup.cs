using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeachMe.Startup))]
namespace TeachMe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
