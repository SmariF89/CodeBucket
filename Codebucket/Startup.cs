using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Codebucket.Startup))]
namespace Codebucket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
