using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using ApplicationFunction = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;


namespace ApiCustomFilters.OwinMiddleware
{
    public class SecurityComponent
    {
        private readonly ApplicationFunction _nextComponent;

        public SecurityComponent(ApplicationFunction appFunc)
        {
            if (appFunc == null) throw new ArgumentNullException("AppFunc of next component");
            _nextComponent = appFunc;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            OwinContext owinContext = new OwinContext(environment);
            IPrincipal principalAtOwinStart = owinContext.Request.User;
            Debug.WriteLine(string.Format("Principal is authenticated at the start of OWIN security middleware: {0}", principalAtOwinStart.Identity.IsAuthenticated));
            await _nextComponent(environment);
        }
    }
}