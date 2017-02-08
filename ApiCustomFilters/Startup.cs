using ApiCustomFilters.OwinMiddleware;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCustomFilters
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Use<SecurityComponent>();
        }
    }
}