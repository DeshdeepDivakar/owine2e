using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Hosting;

[assembly: OwinStartup(typeof(Sample.Web.Startup))]

namespace Sample.Web
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // Configure Web API for self-host. 
      HttpConfiguration config = new HttpConfiguration();
      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );

      app.UseWebApi(config); 
    }
  }
}
