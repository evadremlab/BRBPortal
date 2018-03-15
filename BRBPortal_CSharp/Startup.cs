using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BRBPortal_CSharp.Startup))]
namespace BRBPortal_CSharp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
