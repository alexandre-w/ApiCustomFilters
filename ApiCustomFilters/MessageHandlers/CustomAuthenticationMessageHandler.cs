using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ApiCustomFilters.MessageHandlers
{
    public class CustomAuthenticationMessageHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IPrincipal incomingPrincipal = request.GetRequestContext().Principal;
            Debug.WriteLine(string.Format("Principal is authenticated at the start of SendAsync in CustomAuthenticationMessageHandler: {0}", incomingPrincipal.Identity.IsAuthenticated));
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                IPrincipal genericPrincipal = new GenericPrincipal(new GenericIdentity("Alex", "CustomIdentification"), new string[] { "Admin", "PowerUser" });
                request.GetRequestContext().Principal = genericPrincipal;
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> SendError(string error, HttpStatusCode code)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(error);
            response.StatusCode = code;
            return Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }
    }
}