using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(Online_Cybersecurity_System.Startup))]

namespace Online_Cybersecurity_System
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new CookieAuthenticationOptions
            {
                AuthenticationType = "AUTH", //id of authentication
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout")
            };
            app.UseCookieAuthentication(options);
        }
    }
}
