using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(t_ag.Startup))]
namespace t_ag
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
        }
    }
}
