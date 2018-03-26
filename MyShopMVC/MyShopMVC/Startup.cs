using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyShopMVC.Startup))]
namespace MyShopMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
