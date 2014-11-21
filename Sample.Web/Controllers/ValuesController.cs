using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Sample.Web.Controllers
{
  public class ValuesController:ApiController
  {
    public string[] GetValue()
    {
      return new[] { "a", "b" };
    }
  }
}