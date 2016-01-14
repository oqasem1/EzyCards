using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EzyCards.Startup))]
namespace EzyCards
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
