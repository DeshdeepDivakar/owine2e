using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Web.Tests
{
  [TestClass]
  public class SampleWebTest:OwinE2ETest
  {
    protected override void InitializeWebApp(Owin.IAppBuilder app)
    {
      /// first you need to serve the static files
      /// your application executes in SOLUTION_DIR/Sample.Web.Tests/bin/Debug
      /// therfore you need the ../../../ to navigate to SOLUTION_DIR
      /// I have searched alot to find a better workaround. but there is none to my knowledge.  
      /// You will also have problems if you depend on HostingEnvironment.MapPath() since
      /// it will return null because SelfHost does not configure the Hosting environment correctly. (you will have to find a solution yourself )
      /// 
      ServeVirtualDirectory(app, "", "../../../Sample.Web");
      /// here you initialize your owin app
      new Startup().Configuration(app);
      
    }

    //protected override OpenQA.Selenium.Remote.RemoteWebDriver CreateWebDriver()
    //{
    //  // using chromedriver will allow you to observe what is happening as a window is openend
    //  return new ChromeDriver();
    //}
    /// <summary>
    /// Tests wether the title of home page is correct
    /// </summary>
    /// 

    [TestMethod]
    public void ApplicationBrandShouldBeCorrect()
    {
      /// find elements searches for element to appear within 5 seconds
      Browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

      /// once the control flow is here your web application is being served and you have a web driver with which to perform queries
      /// you could also think about dropping and creating the database in Init() which would allow you to observe the db during brwoser queries
      Browser.Navigate().GoToUrl(Url);
      Browser.FindElementById("loadDataBtn").Click();
            
      var element = Browser.FindElementById("dataArea");
      Assert.AreEqual("a,b", element.Text);
      /// afterwards your application is destroyed as well as the browser.
    }

  }
}
