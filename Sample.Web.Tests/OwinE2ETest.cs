using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System.Net.Sockets;
using System.Net;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;

namespace Sample.Web.Tests
{
  /// <summary>
  /// This base class allows the tester to perform end to end tests of an OWIN web application
  /// 
  /// </summary>
  public abstract class OwinE2ETest
  {
    /// <summary>
    /// gives the test access to an automated web browser
    /// </summary>
    public RemoteWebDriver Browser { get; private set; }

    /// <summary>
    /// This method may be overriden to Generate a free url where to host 
    /// the OWIN web application 
    /// 
    /// Default implementation gets a free port number and returns http://localhost:PORTNUMBER
    /// </summary>
    /// <returns></returns>
    protected virtual string GenerateFreeUrl()
    {
      var listener = new TcpListener(IPAddress.Loopback, 0);
      listener.Start();
      var endPoint = listener.LocalEndpoint as IPEndPoint;
      var port = endPoint.Port;
      listener.Stop();
      return "http://localhost:"+port;
    }


    /// <summary>
    /// user may override to create a remote web driver other than PhantomJS 
    /// e.g. ChromeDriver, FirefoxDriver, IEDriver -> this would require further dependencies in nuget package manager
    /// </summary>
    /// <returns></returns>
    protected virtual RemoteWebDriver CreateWebDriver()
    {
      return new PhantomJSDriver();
    }

    /// <summary>
    /// Initialize method.  Subclasses may create their own initialize method which would have to call Init(). But I suggest you just override Init()
    /// </summary>
    [TestInitialize]
    public void InitializeTest()
    {
      Init();
    }

    /// <summary>
    /// Override this method and call base.Init() if you need to do further per test initialization
    /// 
    /// This method serves your web app (see InitializeWebApp)  and creates a web webdriver which the test may use.
    /// </summary>
    protected virtual void Init()
    {
      this.Url = GenerateFreeUrl();
      this.WebApplication = WebApp.Start(Url, app =>
      {
        InitializeWebApp(app);
      });
      this.Browser = CreateWebDriver();
    }

    /// <summary>
    /// implement this to generate your OWIN app.
    /// 
    /// e.g. new Startup().Configuration(app);
    /// </summary>
    /// <param name="app"></param>
    protected abstract void InitializeWebApp(Owin.IAppBuilder app);


    /// <summary>
    /// allows you to server a specific physical folder as to the specified request Path
    /// </summary>
    /// <param name="app"></param>
    /// <param name="requestPath"></param>
    /// <param name="physicalPath"></param>
    public static void ServeVirtualDirectory(IAppBuilder app, string requestPath, string physicalPath)
    {

      PathString requestPathString;
      if (string.IsNullOrWhiteSpace(requestPath)) requestPathString = PathString.Empty;
      else requestPathString = new PathString(requestPath);

      physicalPath = System.IO.Path.GetFullPath(physicalPath);
      var fileSystem = new PhysicalFileSystem(physicalPath);

      {
        var options = new FileServerOptions()
        {
          EnableDefaultFiles = true,
          RequestPath = requestPathString,
          FileSystem = fileSystem
        };
        app.UseFileServer(options);
      }
    }


    /// <summary>
    /// Per test cleanup method
    /// </summary>
    [TestCleanup]
    public void CleanupTest()
    {
      WebApplication.Dispose();
    }

    /// <summary>
    /// The Url on which the wep app is being served.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// the web application reference (you should not need it)
    /// </summary>
    public IDisposable WebApplication { get; set; }
  }
}
